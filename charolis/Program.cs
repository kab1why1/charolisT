using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.Cookies;
using charolis.Models;
using charolis.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) Регіструємо репозиторії
builder.Services.AddSingleton<RegUserRepository>();
builder.Services.AddSingleton<AdminRepository>();
builder.Services.AddSingleton<GuessRepository>();
builder.Services.AddSingleton<OrdersRepository>();
builder.Services.AddSingleton<ProductRepository>();

// 2) Сервіс аутентифікації
builder.Services.AddScoped<IAuthService, AuthService>();

// 3) MVC
builder.Services.AddControllersWithViews();

// 4) Налаштування cookie‑аутентифікації
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

// 5) Рольова авторизація (за потреби можна звертатися просто через [Authorize(Roles="…")])
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
});

var app = builder.Build();

// 6) Middleware pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();
app.UseRouting();

// ! ВАЖЛИВО: аутентифікацію підключаємо перед авторизацією
app.UseAuthentication();
app.UseAuthorization();

// 7) Роутінг
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UI}/{action=Main}/{id?}");

app.Run();
