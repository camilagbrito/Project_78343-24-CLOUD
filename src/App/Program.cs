using App.Config;
using Azure.Storage.Blobs;
using Business.Interfaces;
using Business.Models;
using Data.Context;
using Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EcomDbContext>(options =>
options.UseSqlServer(connectionString));
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<EcomDbContext>();

builder.Services.AddScoped<EcomDbContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddSingleton(x => new BlobServiceClient(builder.Configuration.GetConnectionString("BlobConnectionString")));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddControllersWithViews();

/*if (builder.Environment.IsDevelopment())
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
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.UseGlobalizationConfig();

app.Run();