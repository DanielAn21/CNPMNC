using LibManager.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LibDbContext>(options =>
  options.UseSqlServer(builder.Configuration.GetConnectionString("sql_server_local") ?? ""));
builder.Services.AddAuthentication("libmanager")
    .AddCookie("libmanager", options =>
    {
        options.AccessDeniedPath = new PathString("/auth/Forbidden");
        options.ExpireTimeSpan = TimeSpan.FromMinutes(1200);
        options.LoginPath = new PathString("/auth/Login");
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
