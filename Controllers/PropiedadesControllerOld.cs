using Microsoft.AspNetCore.Mvc;
using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;

namespace inmobilariaCeli.Controllers;

public class PropiedadesController : Controller
{
    private readonly PropiedadRepository _repo;
    private readonly PropietarioRepository _repoPropietario;

    public PropiedadesController(PropiedadRepository repo, PropietarioRepository repoPropietario)
    {
        _repo = repo;
        _repoPropietario = repoPropietario;
    }

    // GET: Propiedades
    public async Task<IActionResult> Index()
    {
        var propiedades = await _repo.GetAll();
        return View(propiedades);
    }

    // GET: Propiedades/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var propiedad = await _repo.GetById(id);
        if (propiedad == null)
        {
            return NotFound();
        }
        return View(propiedad);
    }

    // GET: Propiedades/Create
    public async Task<IActionResult> Create()
    {
        // Podrías cargar una lista de propietarios para elegir
        ViewBag.Propietarios = await _repoPropietario.GetAll();
        return View();
    }

    // POST: Propiedades/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Propiedad propiedad)
    {
        if (ModelState.IsValid)
        {
            await _repo.AltaPropiedad(propiedad);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Propietarios = await _repoPropietario.GetAll();
        return View(propiedad);
    }

    // GET: Propiedades/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var propiedad = await _repo.GetById(id);
        if (propiedad == null)
        {
            return NotFound();
        }
        ViewBag.Propietarios = await _repoPropietario.GetAll();
        return View(propiedad);
    }

    // POST: Propiedades/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Propiedad propiedad)
    {
        if (id != propiedad.Id)
        {
            return BadRequest();
        }

        if (ModelState.IsValid)
        {
            await _repo.ActualizarPropiedad(propiedad);
            return RedirectToAction(nameof(Index));
        }
        ViewBag.Propietarios = await _repoPropietario.GetAll();
        return View(propiedad);
    }

    // GET: Propiedades/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var propiedad = await _repo.GetById(id);
        if (propiedad == null)
        {
            return NotFound();
        }
        return View(propiedad);
    }

    // POST: Propiedades/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repo.DeletePropiedad(id);
        return RedirectToAction(nameof(Index));
    }
}
