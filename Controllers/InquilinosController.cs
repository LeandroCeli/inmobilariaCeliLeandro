using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobilariaCeli.Controllers
{
    [Authorize]
    public class InquilinosController : Controller
    {
        private readonly InquilinoRepository _repo;

        public InquilinosController(InquilinoRepository repo)
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
        public async Task<IActionResult> Crear(Inquilino i)
        {
            if (!ModelState.IsValid)
                return View(i);

            await _repo.CreateAsync(i);
            return RedirectToAction("Index");
        }

        // ✏️ Editar
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var inquilino = await _repo.GetByIdAsync(id);
            if (inquilino is null) return NotFound();
            return View(inquilino);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Inquilino i)
        {
            if (!ModelState.IsValid)
                return View(i);

            await _repo.UpdateAsync(i);
            return RedirectToAction("Index");
        }

        // ❌ Eliminar
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var inquilino = await _repo.GetByIdAsync(id);
            if (inquilino is null) return NotFound();
            return View(inquilino);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}