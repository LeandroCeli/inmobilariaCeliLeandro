using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace inmobilariaCeli.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        private readonly ContratoRepository _repo;
        private readonly InmuebleRepository _inmuebleRepo;
        private readonly InquilinoRepository _inquilinoRepo;

        public ContratosController(ContratoRepository repo, InmuebleRepository inmuebleRepo, InquilinoRepository inquilinoRepo)
        {
            _repo = repo;
            _inmuebleRepo = inmuebleRepo;
            _inquilinoRepo = inquilinoRepo;
        }

        // 📋 Listar todos
        public async Task<IActionResult> Index()
        {
            var lista = await _repo.GetAllAsync();
            return View(lista);
        }

        // ➕ Crear - GET
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.Inquilinos = await _inquilinoRepo.GetAllAsync();
            return View();
        }

        // ➕ Crear - POST
        [HttpPost]
        public async Task<IActionResult> Create(Contrato c)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Inquilinos = await _inquilinoRepo.GetAllAsync();
                return View(c);
            }

            // ✅ Validación simple: no permitir contrato activo sobre mismo inmueble
            bool haySuperposicion = await _repo.HaySuperposicionAsync(c.IdPropiedad, c.FechaInicio, c.FechaFin);
            if (haySuperposicion)
            {
                ModelState.AddModelError("IdPropiedad", "El inmueble seleccionado ya tiene un contrato activo en las fechas indicadas.");
                ViewBag.Inquilinos = await _inquilinoRepo.GetAllAsync();
                return View(c);
            }

            await _repo.CreateAsync(c);
            return RedirectToAction("Index");
        }

        // ✏️ Editar
        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var contrato = await _repo.GetByIdAsync(id);
            if (contrato is null) return NotFound();

            ViewBag.Inquilinos = await _inquilinoRepo.GetAllAsync();
            return View(contrato);
        }

        [HttpPost]
        public async Task<IActionResult> Editar(Contrato c)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Inquilinos = await _inquilinoRepo.GetAllAsync();
                return View(c);
            }

            await _repo.UpdateAsync(c);
            return RedirectToAction("Index");
        }

        // ❌ Eliminar
        [HttpGet]
        public async Task<IActionResult> Eliminar(int id)
        {
            var contrato = await _repo.GetByIdAsync(id);
            if (contrato is null) return NotFound();
            return View(contrato);
        }

        [HttpPost]
        public async Task<IActionResult> EliminarConfirmado(int id)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        // 🔹 NUEVO - Obtener inmuebles disponibles según tipo y uso
        [HttpGet]
        public async Task<JsonResult> GetInmueblesDisponibles(string tipoInmueble, string tipoUso)
        {
            var inmuebles = await _inmuebleRepo.GetDisponiblesAsync(tipoInmueble, tipoUso);
            return Json(inmuebles);
        }
    }


    
}
