using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;

namespace inmobilariaCeli.Controllers;

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

    // GET: Contratos/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Propiedades = new SelectList(await _repoPropiedad.GetAllAsync(), "Id", "Direccion");
        ViewBag.Inquilinos = new SelectList(await _repoInquilino.GetAllAsync(), "Id", "NombreCompleto");
        return View(new Contrato());
    }

    // POST: Contratos/Create
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

        ViewBag.Propiedades = new SelectList(await _repoPropiedad.GetAllAsync(), "Id", "Direccion", contrato.IdPropiedad);
        ViewBag.Inquilinos = new SelectList(await _repoInquilino.GetAllAsync(), "Id", "NombreCompleto", contrato.IdInquilino);
        return View(contrato);
    }
}