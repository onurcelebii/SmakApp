using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SmakApp.Data;
using SmakApp.Models;
using SmakApp.Services; // IProductService ve ProductService için eklenmiþ

var builder = WebApplication.CreateBuilder(args);

// Servisleri konteynýra ekler.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Oturumun ne kadar süre aktif kalacaðýný belirler.
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()) // Baðlantý hatalarý için yeniden deneme seçeneðini etkinleþtirir.
);

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();

var app = builder.Build();

// HTTP istek boru hattýný yapýlandýrýr.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Hata durumunda özel bir hata sayfasý kullanýr.
    app.UseHsts(); // HTTP Strict Transport Security (HSTS) kullanýr.
}

app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'ye yönlendirir.
app.UseStaticFiles(); // Statik dosyalarý (CSS, JavaScript, resimler vb.) sunar.
app.UseRouting(); // Yönlendirme iþlemlerini baþlatýr.
app.UseAuthentication(); // Kimlik doðrulama middleware'ini ekler.
app.UseAuthorization(); // Yetkilendirme middleware'ini ekler.
app.MapControllers(); // Kontrolleri eþler.

app.UseSession(); // Oturum middleware'ini ekler.

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"); // Varsayýlan rota þablonunu belirler.
});

app.Run(); // Uygulamayý çalýþtýrýr.
