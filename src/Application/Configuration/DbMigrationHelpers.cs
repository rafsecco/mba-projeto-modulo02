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

        var seller = new Seller
        {
            UserId = Guid.Parse(identityUser.Id)
        };
        await context.Sellers.AddAsync(seller);

        await context.SaveChangesAsync();

        if (context.Categories.Any())
            return;

        var category = new Category
        {
            Name = "Flores",
            Description = "Categoria destinada para produtos do tipo flores"
        };

        await context.Categories.AddAsync(category);

        await context.SaveChangesAsync();

        if (context.Products.Any())
            return;

        await context.Products.AddAsync(new Product
        {
            Name = "Rosa",
            Description = "Produto do tipo flores ",
            Price = 25.80m,
            Stock = 100,
            Image = "21.png",

            SellerId = Guid.Parse(identityUser.Id),
            CategoryId = category.Id
        });

        await context.Products.AddAsync(new Product
        {
            Name = "Rosa variação",
            Description = "Produto do tipo flores",
            Price = 15.20m,
            Stock = 150,
            Image = "23.png",

            SellerId = Guid.Parse(identityUser.Id),
            CategoryId = category.Id
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
