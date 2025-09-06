
using Microsoft.AspNetCore.Mvc;
using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;

namespace inmobilariaCeli.Controllers;

public class PropietariosController : Controller
{
    private readonly PropietarioRepository _repo;
    public PropietariosController(PropietarioRepository repo) => _repo = repo;


    public async Task<IActionResult> Index() => View(await _repo.GetAll());


    public async Task<IActionResult> Details(int id)
    {
        var p = await _repo.GetById(id);
        if (p == null) return NotFound();
        return View(p);
    }


    public IActionResult Create() => View();


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Propietario p)
    {
        if (!ModelState.IsValid) return View(p);
        await _repo.AltaPropietario(p);
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Edit(int id)
    {
        var p = await _repo.GetById(id);
        if (p == null) return NotFound();
        return View(p);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Propietario p)
    {
        if (id != p.Id) return BadRequest();
        if (!ModelState.IsValid) return View(p);
        await _repo.ActualizarPropietario(p);
        return RedirectToAction(nameof(Index));
    }


    public async Task<IActionResult> Delete(int id)
    {
        var p = await _repo.GetById(id);
        if (p == null) return NotFound();
        return View(p);
    }


    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repo.DeletePropietario(id);
        return RedirectToAction(nameof(Index));
    }
}
