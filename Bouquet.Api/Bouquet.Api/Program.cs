using Bouquet.Api.Extensions;
using Bouquet.Api.Middlewares;
using Bouquet.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

StripeConfiguration.ApiKey = configuration["StripeApiKey"]?.ToString();

#region Add Logger

builder.AddLogger(configuration);

#endregion

#region EF

builder.Services.AddDbContext<BouquetContext>(options => options.UseSqlServer(configuration.GetConnectionString("ConnectionString")));

#endregion

#region Dependency Injection

builder.InjectServices(configuration);

#endregion

#region Hangfire

builder.AddHangFire(configuration);

#endregion

#region AutoMapper

builder.AddAutoMapper();

#endregion

#region Identity

builder.AddIdentity();

#endregion

#region Authentication

builder.AddJWTAuthentication(configuration);

#endregion

#region Localization

builder.AddLocalization();

#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

#region CORS

app.UseCors(builder =>
{
    builder.AddCORS(configuration);
});

#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<RequestCultureMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Files")),
    RequestPath = "/Files"
});

app.UseAuthentication();
app.UseAuthorization();

#region Database Initialization

await app.InitalizeDatabase();

#endregion

app.ConfigureHangFire(configuration);

app.MapControllers();

app.Run();
