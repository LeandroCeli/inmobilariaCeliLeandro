using inmobilariaCeli.Data;
using inmobilariaCeli.Repositories;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor();

// 👉 Servicios MVC
builder.Services.AddControllersWithViews();

// ✅ Registro de DbConnectionFactory con cadena de conexión
builder.Services.AddScoped<DbConnectionFactory>(provider =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    return new DbConnectionFactory(connectionString);
});

// ✅ Repositorios
builder.Services.AddScoped<PropietarioRepository>();
builder.Services.AddScoped<InquilinoRepository>();
builder.Services.AddScoped<ContratoRepository>();
builder.Services.AddScoped<InmuebleRepository>();
builder.Services.AddScoped<UsuarioRepository>(); 
builder.Services.AddTransient<PropiedadRepository>();


// ✅ Autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Usuarios/AccesoDenegado";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

// ✅ Autorización
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdminPuedeEliminar", policy =>
        policy.RequireRole("Administrador"));
});

var app = builder.Build();

// 👉 Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection(); // opcional
app.UseStaticFiles();
app.UseRouting();

// ✅ Middleware de autenticación y autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();