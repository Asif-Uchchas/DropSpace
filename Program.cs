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
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using DropSpace.Helpers.LinkEncrypt;
using DropSpace.ERPServices.MobilePhoneValidation.Interfaces;
using DropSpace.ERPServices.MobilePhoneValidation;

var builder = WebApplication.CreateBuilder(args);
#region Configure Services
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});
// Add services to the container.
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
    //options.JsonSerializerOptions.MaxDepth = 10;
    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
}).AddRazorRuntimeCompilation();

#endregion
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
#region Auth Related Settings
builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 4;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(1);
    options.Lockout.MaxFailedAccessAttempts = 20;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    options.User.RequireUniqueEmail = false;
});
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromHours(1);

    options.LoginPath = "/Home/Home/Index";
    //options.LoginPath = "/PublicUser/Index";
    options.AccessDeniedPath = "/Home/Home/AccessDenied";
    options.SlidingExpiration = true;
});
#endregion


#region Register application-specific services
builder.Services.AddScoped<IDapper, DropSpace.Services.Dapper.Dapper>();
builder.Services.AddScoped<IUserInfoes, UserInfoes>();
builder.Services.AddScoped<IPersonData, PersonData>();
builder.Services.AddScoped<IMasterData, MasterDataService>();
builder.Services.AddScoped<IMobilePhoneValidation, MobilePhoneValidationService>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
#endregion
// Add HttpClient and WebHostEnvironment
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
builder.Services.AddDistributedMemoryCache();

#region App metrics
builder.Services.Configure<KestrelServerOptions>(opt => { opt.AllowSynchronousIO = true; });
//builder.Services.AddMetrics();
#endregion
builder.Services.AddHttpContextAccessor();
builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".AdventureWorks.Session";
    options.IdleTimeout = TimeSpan.FromHours(24);
    options.Cookie.IsEssential = true;
});
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

#region Areas Config
builder.Services.Configure<RazorViewEngineOptions>(options =>
{
    options.AreaViewLocationFormats.Clear();
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Areas/{2}/Views/Shared/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Views/Shared/{0}.cshtml");
});
#endregion

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
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseSession();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseEncryptDecryptQueryStringsMiddleware();

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Home}/{controller=Home}/{action=Index}/{id?}");

app.Run();