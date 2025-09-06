using inmobilariaCeli.Data;
using inmobilariaCeli.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1) Activar MVC con controladores y vistas
builder.Services.AddControllersWithViews();

// 2) Leer cadena de conexión desde appsettings.json
var connectionString = builder.Configuration.GetConnectionString("MariaDB")!;

// 3) Registrar fábrica de conexión
builder.Services.AddSingleton(new DbConnectionFactory(connectionString));

// 4) Registrar repositorios
builder.Services.AddScoped<PropietarioRepository>();
builder.Services.AddScoped<InquilinoRepository>();

var app = builder.Build();

// 5) Configuración de middleware
if (!app.Environment.IsDevelopment())
{
//    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
//app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
