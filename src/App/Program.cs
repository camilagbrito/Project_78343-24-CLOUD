using App.Config;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Business.Interfaces;
using Data.Context;
using Data.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using Newtonsoft.Json.Linq;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
            .AddEntityFrameworkStores<EcomDbContext>()
            .AddDefaultTokenProviders();

builder.Services.AddScoped<EcomDbContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllersWithViews();

if (builder.Environment.IsDevelopment())
{
    var keyVaultUrl = builder.Configuration.GetSection("KeyVault:KeyVaultURl");
    var keyVaultClientId = builder.Configuration.GetSection("KeyVault:ClientId");
    var keyVaultClientSecret = builder.Configuration.GetSection("KeyVault:ClientSecret");
    var keyVaultDirectoryID = builder.Configuration.GetSection("KeyVault:DirectoryID");

    var credential = new ClientSecretCredential(keyVaultDirectoryID.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value!.ToString());
    
    builder.Configuration.AddAzureKeyVault(keyVaultUrl.Value!.ToString(), keyVaultClientId.Value!.ToString(), keyVaultClientSecret.Value.ToString(), new DefaultKeyVaultSecretManager());
    
    var client = new SecretClient(new Uri(keyVaultUrl.Value!.ToString()), credential);

    builder.Services.AddDbContext<EcomDbContext>(options =>
    options.UseSqlServer(client.GetSecret("AzConnection").Value.Value.ToString()));
}

/*if (builder.Environment.IsDevelopment())
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    builder.Services.AddDbContext<EcomDbContext>(options =>
    options.UseSqlServer(connectionString));
}*/





var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();


app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.UseGlobalizationConfig();

app.Run();