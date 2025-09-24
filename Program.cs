using inmobilariaCeli.Data;
using inmobilariaCeli.Repositories;

var builder = WebApplication.CreateBuilder(args);

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
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();