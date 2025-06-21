using Core.Configuration;
using Core.Data;
using Core.Data.Repositories;
using Core.Services;
using Core.Utils;
using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Reflection;

namespace Core;

public static class DependencyInjection
{
    public static void AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.AddCommands();
        builder.AddDatabase();
        builder.AddRepositoriesAndServices();
        AddCulture();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<Upload>();
    }

    private static void AddCommands(this WebApplicationBuilder builder)
    {
        builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Services.AddMediatR(configuration =>
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
    }

    private static void AddDatabase(this WebApplicationBuilder builder)
    {
        builder.AddDatabaseSelector();

        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddDefaultIdentity<IdentityUser>(options =>
            {
                // Password settings
                //options.Password.RequireDigit = true;
                //options.Password.RequiredLength = 8;
                //options.Password.RequireNonAlphanumeric = true;
                //options.Password.RequireUppercase = true;
                //options.Password.RequireLowercase = true;
                //options.Password.RequiredUniqueChars = 6;

                //options.User.AllowedUserNameCharacters = AllowedUserNameCharacters;

                //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                //options.Lockout.MaxFailedAccessAttempts = 3;

                options.SignIn.RequireConfirmedEmail = false;
                options.User.RequireUniqueEmail = true;
            })
			.AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();
    }

    private static void AddRepositoriesAndServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IProductRepository, ProductRepository>();
        builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        builder.Services.AddScoped<ISellerRepository, SellerRepository>();
        builder.Services.AddScoped<ICategoryService, CategoryService>();
        builder.Services.AddScoped<IProductService, ProductService>();
        builder.Services.AddScoped<IUserService, UserService>();
    }

    private static void AddCulture()
    {
        var cultureInfo = new CultureInfo("pt-BR");
        CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
        CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;
    }
}
