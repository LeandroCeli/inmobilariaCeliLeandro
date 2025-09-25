using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;

namespace inmobilariaCeli.Controllers
{
    public class ContratosController : Controller
    {
        private readonly ContratoRepository _repo;
        private readonly InmuebleRepository _repoPropiedad;
        private readonly InquilinoRepository _repoInquilino;

        public ContratosController(ContratoRepository repo, InmuebleRepository repoPropiedad, InquilinoRepository repoInquilino)
        {
            _repo = repo;
            _repoPropiedad = repoPropiedad;
            _repoInquilino = repoInquilino;
        }

        // GET: Contratos
        public async Task<IActionResult> Index()
        {
            var contratos = await _repo.GetAll();
            return View(contratos);
        }

        // GET: Contratos/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var contrato = await _repo.GetById(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return View(contrato);
        }

        // GET: Contratos/Create
      public async Task<IActionResult> Create()
{
    var propiedades = await _repoPropiedad.GetAllAsync() ?? new List<Inmueble>();
    var inquilinos = await _repoInquilino.GetAllAsync() ?? new List<Inquilino>();

    ViewBag.Propiedades = new SelectList(propiedades, "Id", "Direccion");
    ViewBag.Inquilinos = new SelectList(inquilinos, "Id", "NombreCompleto");

    return View(new Contrato());
}

[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> Create(Contrato contrato)
{
    if (contrato.FechaFin <= contrato.FechaInicio)
    {
        ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la de inicio.");
    }

    if (ModelState.IsValid)
    {
        await _repo.AltaContrato(contrato);
        return RedirectToAction(nameof(Index));
    }

    var propiedades = await _repoPropiedad.GetAllAsync() ?? new List<Inmueble>();
    var inquilinos = await _repoInquilino.GetAllAsync() ?? new List<Inquilino>();

    ViewBag.Propiedades = new SelectList(propiedades, "Id", "Direccion", contrato.IdPropiedad);
    ViewBag.Inquilinos = new SelectList(inquilinos, "Id", "NombreCompleto", contrato.IdInquilino);

    return View(contrato);
}

        // GET: Contratos/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var contrato = await _repo.GetById(id);
            if (contrato == null)
            {
                return NotFound();
            }

            var propiedades = await _repoPropiedad.GetAllAsync() ?? new List<Inmueble>();
            var inquilinos = await _repoInquilino.GetAllAsync() ?? new List<Inquilino>();

            ViewBag.Propiedades = new SelectList(propiedades, "Id", "Direccion", contrato.IdPropiedad);
            ViewBag.Inquilinos = new SelectList(inquilinos, "Id", "NombreCompleto", contrato.IdInquilino);

            return View(contrato);
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Contrato contrato)
        {
            if (id != contrato.Id)
            {
                return BadRequest();
            }

            if (contrato.FechaFin <= contrato.FechaInicio)
            {
                ModelState.AddModelError("FechaFin", "La fecha de fin debe ser posterior a la de inicio.");
            }

            if (ModelState.IsValid)
            {
                await _repo.ActualizarContrato(contrato);
                return RedirectToAction(nameof(Index));
            }

            var propiedades = await _repoPropiedad.GetAllAsync() ?? new List<Inmueble>();
            var inquilinos = await _repoInquilino.GetAllAsync() ?? new List<Inquilino>();

            ViewBag.Propiedades = new SelectList(propiedades, "Id", "Direccion", contrato.IdPropiedad);
            ViewBag.Inquilinos = new SelectList(inquilinos, "Id", "NombreCompleto", contrato.IdInquilino);

            return View(contrato);
        }

        // GET: Contratos/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var contrato = await _repo.GetById(id);
            if (contrato == null)
            {
                return NotFound();
            }
            return View(contrato);
        }

        // POST: Contratos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _repo.DeleteContrato(id);
            return RedirectToAction(nameof(Index));
        }
    }
}