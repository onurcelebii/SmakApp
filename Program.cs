using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using SmakApp.Data;
using SmakApp.Models;
using SmakApp.Services; // IProductService ve ProductService i�in eklenmi�

var builder = WebApplication.CreateBuilder(args);

// Servisleri konteyn�ra ekler.
builder.Services.AddControllersWithViews();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(20); // Oturumun ne kadar s�re aktif kalaca��n� belirler.
});
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure()) // Ba�lant� hatalar� i�in yeniden deneme se�ene�ini etkinle�tirir.
);

builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IInvitationService, InvitationService>();

var app = builder.Build();

// HTTP istek boru hatt�n� yap�land�r�r.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Hata durumunda �zel bir hata sayfas� kullan�r.
    app.UseHsts(); // HTTP Strict Transport Security (HSTS) kullan�r.
}

app.UseHttpsRedirection(); // HTTP isteklerini HTTPS'ye y�nlendirir.
app.UseStaticFiles(); // Statik dosyalar� (CSS, JavaScript, resimler vb.) sunar.
app.UseRouting(); // Y�nlendirme i�lemlerini ba�lat�r.
app.UseAuthentication(); // Kimlik do�rulama middleware'ini ekler.
app.UseAuthorization(); // Yetkilendirme middleware'ini ekler.
app.MapControllers(); // Kontrolleri e�ler.

app.UseSession(); // Oturum middleware'ini ekler.

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"); // Varsay�lan rota �ablonunu belirler.
});

app.Run(); // Uygulamay� �al��t�r�r.
