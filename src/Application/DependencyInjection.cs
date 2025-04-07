using Application.Data;
using Application.Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class DependencyInjection
{
    private const string AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+ ";

    public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommands();
        services.AddDatabase(configuration);
    }

    private static void AddCommands(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    private static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddDatabaseDeveloperPageExceptionFilter();

        services.AddDefaultIdentity<IdentityUser>(options =>
            {
                // Password settings
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequiredUniqueChars = 6;

                options.User.AllowedUserNameCharacters = AllowedUserNameCharacters;

                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //options.Lockout.MaxFailedAccessAttempts = 3;

                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    public static void SeedData(this IApplicationBuilder builder)
    {
        using var scope = builder.ApplicationServices.CreateScope();
        var dbcontext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        // if (!dbcontext.Database.EnsureCreated()) return;

        dbcontext.Database.Migrate();

        var identityUser = new IdentityUser
        {
            Email = "user@email.com",
            UserName = "user@email.com",
        };

        var result = userManager.CreateAsync(identityUser, "Dev@123").GetAwaiter().GetResult();
        if (result.Succeeded)
        {
            var seller = new Seller
            {
                UserId = Guid.Parse(identityUser.Id)
            };
            dbcontext.Sellers.AddAsync(seller).GetAwaiter().GetResult();
            dbcontext.SaveChangesAsync().GetAwaiter().GetResult();
        }
    }
}