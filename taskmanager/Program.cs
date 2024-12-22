﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using taskmanager.Components;
using taskmanager.Model;
using taskmanager.Services;

var builder = WebApplication.CreateBuilder(args);
// ADD Database dependency injection
builder.Services.AddDbContext<Db>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI برای کلاس‌های services
builder.Services.AddScoped<UserServices>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
    {
        options.SignIn.RequireConfirmedAccount = false; // تنظیمات دلخواه برای Identity
        options.Password.RequiredLength = 6; // حداقل طول رمز عبور
        options.Password.RequireNonAlphanumeric = false; // عدم نیاز به کاراکتر خاص
        options.Password.RequireLowercase = false; // عدم نیاز به حروف کوچک
        options.Password.RequireUppercase = false; // عدم نیاز به حروف بزرگ
        options.Password.RequireDigit = false; // عدم نیاز به اعداد
    })
    .AddEntityFrameworkStores<Db>()
    .AddDefaultTokenProviders();
// تنظیم مسیر صفحه لاگین
builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Login"; // مسیر صفحه لاگین
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);

});
// Add services to the container.
builder.Services.AddRazorComponents();

builder.Services.AddServerSideBlazor(); // Blazor Server

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.UseAuthentication(); // حتماً قبل از Authorization فراخوانی شود
app.UseAuthorization();

app.MapRazorComponents<App>();

app.Run();