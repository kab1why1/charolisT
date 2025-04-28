using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using charolis.DAL;
using charolis.Data;
using charolis.Entities;
using charolis.Entity;
using charolis.Services.Implementation;
using charolis.Services.Interfaces;
using charolis.Storage; // Для AdminInitializer

var builder = WebApplication.CreateBuilder(args);

// JSON-сховище
builder.Services.AddSingleton<IDataStorage<User>>(_ => new JsonStorage<User>("Data/users.json"));
builder.Services.AddSingleton<IDataStorage<Product>>(_ => new JsonStorage<Product>("Data/products.json"));
builder.Services.AddSingleton<IDataStorage<Order>>(_ => new JsonStorage<Order>("Data/orders.json"));
builder.Services.AddSingleton<IDataStorage<OrderItem>>(_ => new JsonStorage<OrderItem>("Data/OrderItems.json"));

// Репозиторії
builder.Services.AddScoped<GenericRepository<User>>();
builder.Services.AddScoped<GenericRepository<Product>>();
builder.Services.AddScoped<GenericRepository<Order>>();
builder.Services.AddScoped<GenericRepository<OrderItem>>();

// Сервіси
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
    });

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    AdminInitializer.Initialize(userService);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();