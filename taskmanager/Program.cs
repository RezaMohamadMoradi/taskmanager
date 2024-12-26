using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using taskmanager.Components;
using taskmanager.Model;
using taskmanager.Services;
using static System.Runtime.InteropServices.JavaScript.JSType;

var builder = WebApplication.CreateBuilder(args);
// ADD Database dependency injection
builder.Services.AddDbContext<Db>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// DI برای کلاس‌های services
builder.Services.AddScoped<UserServices>();
builder.Services.AddScoped<TaskServices>();

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
    options.ExpireTimeSpan = TimeSpan.FromDays(1);

});

builder.Services.AddRazorPages()
    .AddRazorPagesOptions(options =>
    {
        options.Conventions.ConfigureFilter(new IgnoreAntiforgeryTokenAttribute());
    });

builder.Services.AddAntiforgery();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpContextAccessor();

builder.Logging.SetMinimumLevel(LogLevel.Debug);
builder.Logging.AddConsole();

builder.Services.AddServerSideBlazor(); // Blazor Server



builder.Services.AddHttpClient();

builder.Services.AddMemoryCache();
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

app.UseAuthentication(); // حتماً قبل از Authorization فراخوانی شود

app.UseAuthorization();
app.UseAntiforgery();


app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
