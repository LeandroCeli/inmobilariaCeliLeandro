using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobilariaCeli.Controllers
{
    [Authorize]
    public class PropietariosController : Controller
    {
        private readonly PropietarioRepository _repo;

        public PropietariosController(PropietarioRepository repo)
        {
            _repo = repo;
        }

        // 📋 Listar todos
        public async Task<IActionResult> Index()
        {
            var lista = await _repo.GetAllConEstadoEliminacionAsync();
            return View(lista);
        }

        // ➕ Crear
        [HttpGet]
        public IActionResult Crear() => View();

        [HttpPost]
        public async Task<IActionResult> Crear(Propietario p)
        {
            if (!ModelState.IsValid)
                return View(p);

            await _repo.AltaPropietario(p);
            return RedirectToAction("Index");
        }

        // ✏️ Editar
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var propietario = await _repo.GetById(id);
            if (propietario is null) return NotFound();
            return View(propietario);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Propietario p)
        {
            if (!ModelState.IsValid)
                return View(p);

            await _repo.ActualizarPropietario(p);
            return RedirectToAction("Index");
        }

        // ❌ Eliminar
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var propietario = await _repo.GetById(id);
            if (propietario is null) return NotFound();
            return View(propietario);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _repo.DeletePropietario(id);
            return RedirectToAction("Index");
        }
    }
}