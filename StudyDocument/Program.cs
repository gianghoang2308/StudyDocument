using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StudyDocument.Models;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container
builder.Services.AddControllersWithViews();

// Enable session
builder.Services.AddDistributedMemoryCache();
//*******
// Program.cs hoặc Startup.cs - trong phương thức ConfigureServices

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(options =>
{
   
    options.DefaultAuthenticateScheme = "UserRole";
    options.DefaultChallengeScheme = "UserRole";
    options.DefaultSignInScheme = "UserRole";
})

.AddCookie("AdminRole", options =>
{
    options.Cookie.Name = "admin.auth.cookie";
    options.LoginPath = "/Admin/Login";       
    options.LogoutPath = "/Admin/Logout";     
    options.SlidingExpiration = true;
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
})

.AddCookie("UserRole", options =>
{
    options.Cookie.Name = "user.auth.cookie";
    options.LoginPath = "/User/Login";       
    options.LogoutPath = "/User/Logout";      
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Lax;
});

//********


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireAuthenticatedUser().AddAuthenticationSchemes("AdminRole").RequireRole("Admin"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireAuthenticatedUser().AddAuthenticationSchemes("UserRole").RequireRole("User"));
});

//*******
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
});

// Configure the database context
builder.Services.AddDbContext<StudyPlatform_BkapContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configure Kestrel for file upload limit (e.g., 50MB)
builder.WebHost.ConfigureKestrel(options =>
{
    options.Limits.MaxRequestBodySize = 50L * 1024 * 1024; // 50MB
});

var app = builder.Build();

//search term
app.MapControllerRoute(
    name: "search",
    pattern: "Search",
    defaults: new { controller = "Search", action = "Index" });

// Use session middleware
app.UseSession();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
// Enable authorization
app.UseAuthorization();

// Map controller routes
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
