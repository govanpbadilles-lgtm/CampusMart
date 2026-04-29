using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CampusMart.Data;
using CampusMart.Models.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.Configure<Microsoft.AspNetCore.Mvc.Razor.RazorViewEngineOptions>(options =>
{
    options.ViewLocationFormats.Add("/Views/User/{1}/{0}.cshtml");
    options.ViewLocationFormats.Add("/Views/Admin/{1}/{0}.cshtml");

    // Important: Admin controllers are inside an MVC Area ("Admin").
    // By default, ASP.NET searches /Areas/Admin/Views/... which doesn't exist in this project.
    // Add these locations so it can find your existing /Views/Admin/... folder.
    options.AreaViewLocationFormats.Add("/Views/Admin/{1}/{0}.cshtml");
    options.AreaViewLocationFormats.Add("/Views/User/{1}/{0}.cshtml");
});

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequiredLength = 6;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/Login";
});

var app = builder.Build();

// Seed a static admin user (DEV-only).
// The admin credentials you provided are stored here for convenience.
// Identity hashes the password; we still recommend replacing this with a safer approach for production.
if (app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var db = services.GetRequiredService<AppDbContext>();
        await db.Database.MigrateAsync();

        var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

        const string adminRole = "Admin";
        if (!await roleManager.RoleExistsAsync(adminRole))
        {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        // You requested:
        // admin id: 23769862
        // admin pass: Marecigan@0912
        var adminId = "23769862";
        var adminPass = "Marecigan@0912";
        var adminFullName = "Admin";

        var adminUser = await userManager.FindByNameAsync(adminId);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminId, // your app stores StudentId as UserName
                Email = $"{adminId}@campusmart.local",
                FullName = adminFullName,
                StudentId = adminId,
                DateTime = DateTime.UtcNow,
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPass);
            if (!createResult.Succeeded)
            {
                // Fail loudly during dev so you notice misconfiguration.
                throw new InvalidOperationException(
                    $"Failed to create admin user: {string.Join(", ", createResult.Errors.Select(e => e.Description))}"
                );
            }
        }

        if (!await userManager.IsInRoleAsync(adminUser, adminRole))
        {
            await userManager.AddToRoleAsync(adminUser, adminRole);
        }
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=HomeIndex}/{id?}")
    .WithStaticAssets();


app.Run();
