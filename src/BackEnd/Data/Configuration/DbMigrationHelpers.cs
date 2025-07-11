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
			await EnsureSeedRolesAclaims(context, roleManager);
			await EnsureUsers(context, userManager);
			await EnsureSeedProducts(context, userManager);
		}
	}

	private static async Task CreateUserWithRoleAsync(ApplicationDbContext context, UserManager<IdentityUser> userManager, string email, string password, string roleName)
	{
		var user = new IdentityUser
		{
			Email = email,
			UserName = email
		};
		var result = await userManager.CreateAsync(user, password);
		if (result.Succeeded)
		{
			await userManager.AddToRoleAsync(user, roleName);
		}
		else
		{
			throw new Exception($"Falha ao criar o usuário identity {email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
		}

		switch (roleName)
		{
			case "Admin": break;
			case "Vendedor":
				var vendedor = new Vendedor
				{
					UserId = Guid.Parse(user.Id)
				};
				await context.Vendedores.AddAsync(vendedor);
				break;
			case "Cliente":
				var cliente = new Cliente
				{
					UserId = Guid.Parse(user.Id)
				};
				await context.Clientes.AddAsync(cliente);
				break;
			default:
				throw new Exception($"Tipo de usuário não encontrado!");
		}
		await context.SaveChangesAsync();
	}

	private static async Task EnsureUsers(ApplicationDbContext context, UserManager<IdentityUser> userManager)
	{
		if (context.Users.Any())
			return;

		await CreateUserWithRoleAsync(context, userManager, "admin@mail.com", "Dev@123", "Admin");
		await CreateUserWithRoleAsync(context, userManager, "vendedor1@mail.com", "Dev@123", "Vendedor");
		await CreateUserWithRoleAsync(context, userManager, "vendedor2@mail.com", "Dev@123", "Vendedor");
		await CreateUserWithRoleAsync(context, userManager, "cliente@mail.com", "Dev@123", "Cliente");
	}

	private static async Task EnsureSeedProducts(ApplicationDbContext context, UserManager<IdentityUser> userManager)
	{
		#region Criar Categorias
		if (context.Categorias.Any())
			return;

		var categorias = new List<Categoria>
		{
			new Categoria { Nome = "Flores", Descricao = "Categoria destinada para produtos do tipo flores" },
			new Categoria { Nome = "Limpeza", Descricao = "Categoria destinada para produtos do tipo limpeza" }
		};

		await context.Categorias.AddRangeAsync(categorias);
		await context.SaveChangesAsync();
		#endregion Criar Categorias

		#region Criar Produtos
		if (context.Produtos.Any())
			return;

		var produtos = new Produto[]
		{
			new() {
				Nome = "Rosa",
				Descricao = "Produto do tipo flores",
				Preco = 25.80m,
				Estoque = 100,
				Imagem = "21.png",
				VendedorId = Guid.Parse(userManager.FindByEmailAsync("vendedor1@mail.com").Result.Id),
				CategoriaId = context.Categorias.FirstOrDefault(c => c.Nome == "Flores")?.Id ?? Guid.Empty
			},
			new() {
				Nome = "Rosa variação",
				Descricao = "Produto do tipo flores",
				Preco = 15.20m,
				Estoque = 150,
				Imagem = "23.png",
				VendedorId = Guid.Parse(userManager.FindByEmailAsync("vendedor1@mail.com").Result.Id),
				CategoriaId = context.Categorias.FirstOrDefault(c => c.Nome == "Flores")?.Id ?? Guid.Empty
			},


			new() {
				Nome = "Orquídeas",
				Descricao = "Produto do tipo flores",
				Preco = 20.85m,
				Estoque = 100,
				Imagem = "21.png",
				VendedorId = Guid.Parse(userManager.FindByEmailAsync("vendedor2@mail.com").Result.Id),
				CategoriaId = context.Categorias.FirstOrDefault(c => c.Nome == "Flores")?.Id ?? Guid.Empty
			},
			new() {
				Nome = "Detergente",
				Descricao = "Produto do tipo limpeza",
				Preco = 30.20m,
				Estoque = 80,
				Imagem = "Detergent_307.png",
				VendedorId = Guid.Parse(userManager.FindByEmailAsync("vendedor2@mail.com").Result.Id),
				CategoriaId = context.Categorias.FirstOrDefault(c => c.Nome == "Limpeza")?.Id ?? Guid.Empty
			},
		};
		await context.Produtos.AddRangeAsync(produtos);
		await context.SaveChangesAsync();
		#endregion Criar Produtos
	}

	private static async Task EnsureSeedRolesAclaims(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
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
