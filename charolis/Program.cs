using Microsoft.AspNetCore.Authentication.Cookies;
using charolis.Models;
using charolis.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Реєстрація репозиторіїв
builder.Services.AddSingleton<RegUserRepository>();
builder.Services.AddSingleton<AdminRepository>();
builder.Services.AddSingleton<GuessRepository>();
builder.Services.AddSingleton<OrdersRepository>();
builder.Services.AddSingleton<ProductRepository>();

// 2) Реєстрація сервісу аутентифікації
builder.Services.AddScoped<IAuthService, AuthService>();

// 3) Додавання підтримки MVC із контролерами та представленнями
builder.Services.AddControllersWithViews();

// 4) Налаштування cookie‑аутентифікації
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// 5) Налаштування авторизації (допоміжна політика для адміністратора, якщо потрібна)
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

var app = builder.Build();

// 6) Налаштування middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

// Важливо: спочатку аутентифікація, потім авторизація
app.UseAuthentication();
app.UseAuthorization();

// 7) Налаштування маршрутизації
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UI}/{action=Main}/{id?}");

// 8) Початкове наповнення даних: створення адміністратора, якщо його ще нема
using (var scope = app.Services.CreateScope())
{
    var adminRepo = scope.ServiceProvider.GetRequiredService<AdminRepository>();
    if (!adminRepo.GetAll().Any())
    {
        // Створюємо адміністратора з базовими даними.
        // Зверніть увагу: для виробничого середовища потрібно замінити ці дані на більш безпечні.
        var admin = new Admin("Admin", "admin@example.com", PasswordHelper.Hash("adminpassword"), "123456789", "Адреса");
        adminRepo.Add(admin);
    }
}

app.Run();
