using Bouquet.Api.Authorization;
using Bouquet.Database;
using Bouquet.Database.Entities.Identity;
using Bouquet.Services.Aditional;
using Bouquet.Services.Authentication;
using Bouquet.Services.Bouquet;
using Bouquet.Services.FlowerShop;
using Bouquet.Services.Helpers;
using Bouquet.Services.Interfaces.Aditional;
using Bouquet.Services.Interfaces.Authentication;
using Bouquet.Services.Interfaces.Bouquet;
using Bouquet.Services.Interfaces.FlowerShop;
using Bouquet.Services.Interfaces.Helpers;
using Bouquet.Services.Interfaces.Mail;
using Bouquet.Services.Interfaces.Order;
using Bouquet.Services.Interfaces.Payment;
using Bouquet.Services.Interfaces.Permissions;
using Bouquet.Services.Interfaces.User;
using Bouquet.Services.Mail;
using Bouquet.Services.Models.Mail;
using Bouquet.Services.Order;
using Bouquet.Services.Payments;
using Bouquet.Services.Permissions;
using Bouquet.Services.User;
using Bouquet.Shared.Constants;
using Bouquet.Shared.Models;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace Bouquet.Api.Extensions
{
    public static class ApplicationBuilderExtension
    {

        #region Methods

        /// <summary>
        /// Injects all services that are used by the app
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void InjectServices(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddScoped<IJWTAuthenticationService, JWTAuthenticationService>();
            builder.Services.AddScoped<ITokenHelper, TokenHelper>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IAccountService, AccountService>();
            builder.Services.AddScoped<IPermissionsService, PermissionsService>();
            builder.Services.Configure<MailConfiguration>(configuration.GetSection("MailConfiguration"));
            builder.Services.Configure<URLConfiguration>(configuration.GetSection("URLConfiguration"));
            builder.Services.AddScoped<IUserMailService, UserMailService>();
            builder.Services.AddScoped<IUserMailHelper, UserMailHelper>();
            builder.Services.AddScoped<INotyficationHelper, NotyficationHelper>();
            builder.Services.AddScoped<IMailService, MailService>();
            builder.Services.AddScoped<DatabaseSeeder, DatabaseSeeder>();
            builder.Services.AddScoped<IAuthorizationHandler, RoleClaimsHandler>();
            builder.Services.AddScoped<IFlowerShopService, FlowerShopService>();
            builder.Services.AddScoped<IBouquetService, BouquetService>();
            builder.Services.AddScoped<ICityService, CityService>();
            builder.Services.AddScoped<IColorService, ColorService>();
            builder.Services.AddScoped<IFlowerService, FlowerService>();
            builder.Services.AddScoped<IOrderService, OrderService>();
            builder.Services.AddScoped<ICustomersService, CustomersService>();
            builder.Services.AddScoped<IPaymentsService, PaymentsService>();
            builder.Services.AddScoped<IWalletService, WalletService>();
        }

        /// <summary>
        /// adds hangfire
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void AddHangFire(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddHangfire(config => config
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(configuration.GetConnectionString("ConnectionString"), new SqlServerStorageOptions
                {
                    CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                    SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                    QueuePollInterval = TimeSpan.Zero,
                    UseRecommendedIsolationLevel = true
                }));

            builder.Services.AddHangfireServer();
        }

        /// <summary>
        /// Adds Identity
        /// </summary>
        /// <param name="builder"></param>
        public static void AddIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<BouquetUser, BouquetRole>()
                            .AddEntityFrameworkStores<BouquetContext>()
                            .AddDefaultTokenProviders();
        }

        /// <summary>
        /// Adds JWT authentication
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void AddJWTAuthentication(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                    .AddJwtBearer(options =>
                        {
                            options.SaveToken = true;
                            options.RequireHttpsMetadata = false;
                            options.TokenValidationParameters = new TokenValidationParameters()
                            {
#if DEBUG
                                ValidateIssuer = false,
                                ValidateAudience = false,
#else
                                ValidateIssuer = true,
                                ValidateAudience = true,
#endif
                                ValidateLifetime = true,
                                ValidateIssuerSigningKey = true,
                                ClockSkew = TimeSpan.Zero,
                                ValidAudience = configuration["URLConfiguration:AddressWebClient"],
                                ValidIssuer = configuration["URLConfiguration:AddressAPI"],
                                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
                            };
                        });

            builder.Services.AddAuthorization(
                          options =>
                          {
                              foreach (var prop in typeof(RoleClaim).GetFields(BindingFlags.Public |
                                                                        BindingFlags.Static |
                                                                        BindingFlags.FlattenHierarchy))
                              {
                                  var propertyValue = prop.GetValue(null);
                                  if (propertyValue is not null)
                                  {
                                      options.AddPolicy(
                                                        propertyValue.ToString()!,
                                                        policy => policy.AddRequirements(new RoleClaimsRequirement(propertyValue.ToString()!)));
                                  }
                              }
                          }
                         );
        }

        /// <summary>
        /// Adds localization to the API
        /// </summary>
        /// <param name="builder"></param>
        public static void AddLocalization(this WebApplicationBuilder builder)
        {
            builder.Services.AddLocalization();
            builder.Services.Configure<RequestLocalizationOptions>(
                options =>
                {
                    var supportedCultures = new List<CultureInfo>
                    {
                        new CultureInfo("en"),
                        new CultureInfo("bg")
                    };
                    options.DefaultRequestCulture = new RequestCulture(culture: "en", uiCulture: "en");
                    options.SupportedCultures = supportedCultures;
                    options.SupportedUICultures = supportedCultures;
                });
        }


        /// <summary>
        /// Configures CORS
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void AddCORS(this CorsPolicyBuilder builder, IConfiguration configuration)
        {
            //builder.WithOrigins(configuration["URLConfiguration:AddressWebClient"]!)
            builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
        }

        /// <summary>
        /// Adds Serilog.
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="configuration"></param>
        public static void AddLogger(this WebApplicationBuilder builder, IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration()
                            .ReadFrom.Configuration(configuration)//read the application configuration from the application builder
                            .Enrich.FromLogContext()
                            .CreateLogger();

            builder.Logging.ClearProviders();//We clear all existing logging providers
            builder.Logging.AddSerilog(Log.Logger);
        }

        public static void AddAutoMapper(this WebApplicationBuilder builder)
        {
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }

        /// <summary>
        /// Initialization of database-migrations seeding etc
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static async Task InitalizeDatabase(this WebApplication app)
        {
            using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();

            var context = scope.ServiceProvider.GetRequiredService<BouquetContext>();
            context.Database.Migrate();

            var databaseSeeder = scope.ServiceProvider.GetService<DatabaseSeeder>();
            await databaseSeeder!.SeedRoles();
        }

        #endregion
    }
}
