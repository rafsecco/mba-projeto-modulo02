using Core.Data;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace Core.Configuration;

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

        await EnsureSeedAccessProfile(context, roleManager);
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

    private static async Task EnsureSeedAccessProfile(ApplicationDbContext context, RoleManager<IdentityRole> roleManager)
    {
        await CreateRoleAsync(roleManager, "Admin");
        await CreateRoleAsync(roleManager, "Vendedor");
        await CreateRoleAsync(roleManager, "Cliente");

        await AddRoleClaimAsync(context, roleManager, "Admin", new Claim("Produtos", "AD,VI,ED,EX"));
        await AddRoleClaimAsync(context, roleManager, "Admin", new Claim("Categorias", "AD,VI,ED,EX"));
        await AddRoleClaimAsync(context, roleManager, "Vendedor", new Claim("Produtos", "AD,VI,ED,EX"));
        await AddRoleClaimAsync(context, roleManager, "Cliente", new Claim("Produtos", "VI"));
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

        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
        if (environmentName == Environments.Development)
        {
            roleClaim.Id = GetNextDevId(context);
        }

        context.RoleClaims.Add(roleClaim);
        await context.SaveChangesAsync();
    }

    private static int GetNextDevId(ApplicationDbContext context)
    {
        var maxId = context.RoleClaims.Any() ? context.RoleClaims.Max(c => c.Id) : 0;
        return maxId + 1;
    }
}