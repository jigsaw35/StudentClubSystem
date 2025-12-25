using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using StudentClubSystem.Data;
using StudentClubSystem.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Veritabaný Baðlantýsý
// appsettings.json içindeki "DefaultConnection" ismini kullanýr.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// 2. Repository Baðlantýlarý (Dependency Injection)
// IGenericRepository istendiðinde GenericRepository ver.
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

// 3. Authentication (Giriþ) Ayarlarý
// AccountController ile uyumlu olmasý için varsayýlan þemayý kullanýyoruz.
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        // Kullanýcý giriþ yapmamýþsa yönlendirilecek adres:
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60); // Oturum 60 dk sürsün
    });

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Hata Yönetimi Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 4. Kimlik ve Yetki (Sýrasý Önemli)
app.UseAuthentication(); // Önce: Kimsin?
app.UseAuthorization();  // Sonra: Yetkin var mý?

app.MapControllerRoute(
    name: "default",
    // Home controller yoksa Club/Index olarak açýlmasý daha güvenli
    pattern: "{controller=Club}/{action=Index}/{id?}");

app.Run();