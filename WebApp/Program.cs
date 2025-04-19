using Business.Interfaces;
using Business.Services;
using Data.Contexts;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();

builder.Services.AddDbContext<DataContext>(x => x.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

/* Business -> Services*/
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsersProfileService,  UsersProfileService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

/* Data -> Repositories*/
builder.Services.AddScoped<UsersProfileRepository>();
builder.Services.AddScoped<ClientsRepository>();
builder.Services.AddScoped<ProjectsRepository>();
builder.Services.AddScoped<ProjectUsersRepository>();

builder.Services.AddIdentity<ApplicationUserEntity, IdentityRole>(y =>
    {
        y.User.RequireUniqueEmail = true;
        y.Password.RequiredLength = 8;
    })
    .AddEntityFrameworkStores<DataContext>()
    .AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(x =>
{
    x.LoginPath = "/auth/signinpage";
    x.SlidingExpiration = true;
    x.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    x.Cookie.HttpOnly = true;
    x.Cookie.SameSite = SameSiteMode.None;
    x.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
    options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
});



var app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Admin}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
