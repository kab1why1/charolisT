using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using charolis.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<RegUserRepository>();
builder.Services.AddSingleton<AdminRepository>();
builder.Services.AddSingleton<GuessRepository>();
builder.Services.AddSingleton<OrdersRepository>();
builder.Services.AddSingleton<ProductRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Головна сторінка тепер `AdminController.Menu`
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=UI}/{action=Main}/{id?}");

app.Run();