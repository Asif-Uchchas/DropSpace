using DropSpace.Context;
using DropSpace.Data.Entity;
using DropSpace.ERPService.AuthService.Interfaces;
using DropSpace.ERPServices.AuthService;
using DropSpace.ERPServices.MasterData.Interfaces;
using DropSpace.ERPServices.MasterData;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.ERPServices.PersonData;
using DropSpace.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DropSpace.Services.Dapper;
using Microsoft.AspNetCore.Antiforgery;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using DropSpace.Repository.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add logging configuration
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddDbContext<DropSpaceDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Identity for user and role management
builder.Services.AddIdentity<ApplicationUser, ApplicationRole>()
    .AddEntityFrameworkStores<DropSpaceDbContext>()
    .AddDefaultTokenProviders();

// Configure Kestrel options for large file uploads
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 2147483648; // Set to 2 GB (in bytes)
});

// Add services to the container
builder.Services.AddControllersWithViews();

// Register application-specific services
builder.Services.AddScoped<IDapper, DropSpace.Services.Dapper.Dapper>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserInfoes, UserInfoes>();
builder.Services.AddScoped<IPersonData, PersonData>();
builder.Services.AddScoped<IMasterData, MasterDataService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
// Add HttpClient and WebHostEnvironment
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
var app = builder.Build();
#region For Seed Value
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<ApplicationRole>>();
        var context = services.GetRequiredService<DropSpaceDbContext>();
        await SeedData.SeedAsync(userManager, roleManager, context);
    }
    catch (Exception ex)
    {
        Console.WriteLine($"An error occurred during seeding: {ex.Message}");
    }
}
#endregion
// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Ensure static files middleware is configured



app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}");

app.Run();