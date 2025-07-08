using Business.Models;
using Core.Migrations;
using Data.Context;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Data.Configuration;

public static class DbMigrationHelperExtension
{
	public static void UseDbMigrationHelper(this WebApplication app)
	{
		DbMigrationHelpers.EnsureSeedData(app).Wait();
	}
}

public static class DbMigrationHelpers
{
	public static async Task EnsureSeedData(WebApplication serviceScope)
	{
		var services = serviceScope.Services.CreateScope().ServiceProvider;
		await EnsureSeedData(services);
	}

	public static async Task EnsureSeedData(IServiceProvider serviceProvider)
	{
		using var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope();
		var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();

		var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
		var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
		var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

		if (env.IsDevelopment() || env.IsStaging())
		{
			await context.Database.MigrateAsync();
			await EnsureSeedProducts(context, userManager);
		}

		await EnsureSeedAccessProfile(context, roleManager, userManager);
	}

	private static async Task EnsureSeedProducts(ApplicationDbContext context, UserManager<IdentityUser> userManager)
	{
		if (context.Users.Any())
			return;

		var identityUser = new IdentityUser
		{
			Email = "dev@mail.com",
			UserName = "dev@mail.com"
		};

		var result = userManager.CreateAsync(identityUser, "Dev@123").GetAwaiter().GetResult();
		if (!result.Succeeded) return;

		var vendedor = new Vendedor
		{
			UserId = Guid.Parse(identityUser.Id)
		};
		await context.Vendedores.AddAsync(vendedor);

		await context.SaveChangesAsync();

        var cliente = new Cliente
        {
            UserId = Guid.Parse(identityUser.Id),

            Id = Guid.NewGuid(),
            Ativo = true
        };
        await context.Clientes.AddAsync(cliente);

        await context.SaveChangesAsync();



















        if (context.Categorias.Any())
			return;

		var categoria = new Categoria
		{
			Nome = "Flores",
			Descricao = "Categoria destinada para produtos do tipo flores"
		};

		await context.Categorias.AddAsync(categoria);

		await context.SaveChangesAsync();

		if (context.Produtos.Any())
			return;

		await context.Produtos.AddAsync(new Produto
		{
			Nome = "Rosa",
			Descricao = "Produto do tipo flores ",
			Preco = 25.80m,
			Estoque = 100,
			Imagem = "21.png",

			VendedorId = Guid.Parse(identityUser.Id),
			CategoriaId = categoria.Id
		});

		await context.Produtos.AddAsync(new Produto
		{
			Nome = "Rosa variação",
			Descricao = "Produto do tipo flores",
			Preco = 15.20m,
			Estoque = 150,
			Imagem = "23.png",

			VendedorId = Guid.Parse(identityUser.Id),
			CategoriaId = categoria.Id
		});

		await context.SaveChangesAsync();
	}

	private static async Task EnsureSeedAccessProfile(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
	{
		#region Definindo os roles e claims em um dicionário

		var rolesWithClaims = new Dictionary<string, List<Claim>>
		{
			{ "Admin", new List<Claim>
				{
					new Claim("Produtos", "VI"),
					new Claim("Categorias", "AD,VI,ED,EX")
				}
			},
			{ "Vendedor", new List<Claim>
				{
					new Claim("Produtos", "AD,VI,ED,EX")
				}
			},
			{ "Cliente", new List<Claim>
				{
					new Claim("Produtos", "VI")
				}
			}
		};

		foreach (var role in rolesWithClaims)
		{
			await CreateRoleAsync(roleManager, role.Key);

			foreach (var claim in role.Value)
			{
				await AddRoleClaimAsync(context, roleManager, role.Key, claim);
			}
		}

		#endregion Definindo os roles e claims em um dicionário

		#region Criar usuário admin

		var usuarioAdminIdentity = new IdentityUser
		{
			Email = "admin@mail.com",
			UserName = "admin@mail.com"
		};

		var createUserResult = await userManager.CreateAsync(usuarioAdminIdentity, "Dev@123");

		if (createUserResult.Succeeded)
		{
			var roleAdmin = await roleManager.FindByNameAsync("Admin");

			if (roleAdmin != null)
			{
				var addToRoleResult = await userManager.AddToRoleAsync(usuarioAdminIdentity, "Admin");

				if (addToRoleResult.Succeeded)
				{
					Console.WriteLine("Usuário admin adicionado à role Admin com sucesso.");
				}
				else
				{
					Console.WriteLine("Falha ao adicionar usuário à role Admin.");
				}
			}
			else
			{
				Console.WriteLine("Role 'Admin' não encontrada.");
			}
		}
		else
		{
			Console.WriteLine("Falha ao criar o usuário admin.");
		}

		#endregion Criar usuário admin
	}

	private static async Task CreateRoleAsync(RoleManager<IdentityRole> roleManager, string roleName)
	{
		if (await roleManager.RoleExistsAsync(roleName))
			return;

		var role = new IdentityRole(roleName);
		await roleManager.CreateAsync(role);
	}

	private static async Task AddRoleClaimAsync(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, string roleName, Claim claim)
	{
		var role = await roleManager.FindByNameAsync(roleName);
		if (role == null)
			return;

		var existingClaims = await roleManager.GetClaimsAsync(role);
		if (existingClaims.Any(c => c.Type == claim.Type && c.Value == claim.Value))
			return;

		var roleClaim = new IdentityRoleClaim<string>
		{
			RoleId = role.Id,
			ClaimType = claim.Type,
			ClaimValue = claim.Value
		};

		context.RoleClaims.Add(roleClaim);
		await context.SaveChangesAsync();
	}
}
