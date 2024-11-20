using DropSpace.Context;
using DropSpace.Contracts;
using DropSpace.Data.Entity;
using DropSpace.ERPService.AuthService.Interfaces;
using DropSpace.ERPServices.AuthService;
using DropSpace.ERPServices.MasterData.Interfaces;
using DropSpace.ERPServices.MasterData;
using DropSpace.ERPServices.PersonData.Interfaces;
using DropSpace.ERPServices.PersonData;
using DropSpace.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch.Internal;
using Microsoft.EntityFrameworkCore;
using DropSpace.Services.Dapper;

var builder = WebApplication.CreateBuilder(args);


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

// Add services to the container.
builder.Services.AddControllersWithViews();
// Register application-specific services
builder.Services.AddScoped<IDapper, DropSpace.Services.Dapper.Dapper>();
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserInfoes, UserInfoes>();
builder.Services.AddScoped<IPersonData, PersonData>();
builder.Services.AddScoped<IMasterData, MasterDataService>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
