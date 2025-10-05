using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace inmobilariaCeli.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly UsuarioRepository _usuarioRepo;

        public UsuariosController(UsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        // 🔐 Login
        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {

            var usuario = _usuarioRepo.ObtenerPorEmail(email);
            if (usuario is null || !BCrypt.Net.BCrypt.Verify(password, usuario.Password))
            {
                ModelState.AddModelError("", "Credenciales inválidas");
                return View();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, usuario.Email),
                new Claim("NombreCompleto", usuario.NombreCompleto),
                new Claim(ClaimTypes.Role, usuario.Rol)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
            return RedirectToAction("Index", "Home");
        }

        // 🔓 Logout
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }

        // 🚫 Acceso denegado
        [HttpGet]
        public IActionResult AccesoDenegado() => View();

        // 👤 Perfil del usuario logueado
        [Authorize]
        public IActionResult Perfil()
        {
            var email = User.Identity?.Name;
            var usuario = _usuarioRepo.ObtenerPorEmail(email ?? "");
            return View(usuario);
        }

        // 🔑 Cambiar contraseña
        [HttpPost]
        [Authorize]
        public IActionResult CambiarPassword(string actual, string nueva)
        {
            var email = User.Identity?.Name;
            var usuario = _usuarioRepo.ObtenerPorEmail(email ?? "");

            if (!BCrypt.Net.BCrypt.Verify(actual, usuario.Password))
            {
                ModelState.AddModelError("", "Contraseña actual incorrecta");
                return View("Perfil", usuario);
            }

            var nuevoHash = BCrypt.Net.BCrypt.HashPassword(nueva);
            _usuarioRepo.ActualizarPassword(usuario.Id, nuevoHash);
            return RedirectToAction("Perfil");
        }

        // 🖼️ Cambiar foto de perfil
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CambiarFoto(IFormFile foto)
        {
            var email = User.Identity?.Name;
            var usuario = _usuarioRepo.ObtenerPorEmail(email ?? "");

            if (foto != null && foto.Length > 0)
            {
                var nombreArchivo = Path.GetFileName(foto.FileName);
                var ruta = Path.Combine("wwwroot/img/perfiles", nombreArchivo);
                using var stream = new FileStream(ruta, FileMode.Create);
                await foto.CopyToAsync(stream);
                _usuarioRepo.ActualizarFoto(usuario.Id, "/img/perfiles/" + nombreArchivo);
            }

            return RedirectToAction("Perfil");
        }

        // ❌ Quitar foto de perfil
        [Authorize]
        public IActionResult QuitarFoto()
        {
            var email = User.Identity?.Name;
            var usuario = _usuarioRepo.ObtenerPorEmail(email ?? "");
            _usuarioRepo.QuitarFoto(usuario.Id);
            return RedirectToAction("Perfil");
        }

        // 📋 Listar usuarios (solo Admin)
        [Authorize(Policy = "SoloAdminPuedeEliminar")]
        public IActionResult Index()
        {
            var usuarios = _usuarioRepo.ListarTodos();
            return View(usuarios);
        }

        // ➕ Crear usuario (solo Admin)
        [Authorize(Policy = "SoloAdminPuedeEliminar")]
        [HttpGet]
        public IActionResult Crear() => View();

        [Authorize(Policy = "SoloAdminPuedeEliminar")]
        [HttpPost]
        [HttpPost]
       [HttpPost]
        public async Task<IActionResult> Crear(Usuario usuario, IFormFile FotoPerfil)
        {
            if (ModelState.IsValid)
            {
                return View(usuario);
            }

            // Hashear la contraseña (aunque no se almacene)
           usuario.Password = BCrypt.Net.BCrypt.HashPassword(usuario.Password);
            

            // Procesar imagen si se cargó
            if (FotoPerfil != null && FotoPerfil.Length > 0)
            {
                var nombreArchivo = Path.GetFileName(FotoPerfil.FileName);
                var ruta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img/perfiles", nombreArchivo);

                using var stream = new FileStream(ruta, FileMode.Create);
                await FotoPerfil.CopyToAsync(stream);

                usuario.FotoPerfil = "/img/perfiles/" + nombreArchivo;
            }
             // Guardar el usuario sin datos sensibles
            _usuarioRepo.CrearUsuario(usuario);


            // ⚠️ No almacenar email ni contraseña
            usuario.Email = null;
            usuario.Password = null;

           

            TempData["Mensaje"] = "✅ Usuario creado correctamente.";

            return RedirectToAction("Crear");
        }
    

        // ✏️ Editar usuario (solo Admin)
        [Authorize(Policy = "SoloAdminPuedeEliminar")]
        [HttpGet]
        public IActionResult Editar(int id)
        {
            var usuario = _usuarioRepo.ObtenerPorId(id);
            return View(usuario);
        }

        [Authorize(Policy = "SoloAdminPuedeEliminar")]
        [HttpPost]
        public async Task<IActionResult> Editar(Usuario usuario, IFormFile FotoPerfil)
        {
            if (!ModelState.IsValid)
                return View(usuario);

            if (FotoPerfil != null && FotoPerfil.Length > 0)
            {
                var nombreArchivo = Path.GetFileName(FotoPerfil.FileName);
                var ruta = Path.Combine("wwwroot/img/perfiles", nombreArchivo);
                using var stream = new FileStream(ruta, FileMode.Create);
                await FotoPerfil.CopyToAsync(stream);
                usuario.FotoPerfil = "/img/perfiles/" + nombreArchivo;
            }

            _usuarioRepo.ActualizarUsuario(usuario);
            return RedirectToAction("Index");
        }

        // 🗑️ Eliminar usuario (ya lo tenías)
        [Authorize(Policy = "SoloAdminPuedeEliminar")]
        public IActionResult Eliminar(int id)
        {
            _usuarioRepo.Eliminar(id);
            return RedirectToAction("Index");
        }
    }
}