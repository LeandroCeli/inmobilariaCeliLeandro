using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace inmobilariaCeli.Controllers;

public class InmueblesController : Controller
{
    private readonly InmuebleRepository _repo;
    private readonly PropietarioRepository _propRepo;

    public InmueblesController(InmuebleRepository repo, PropietarioRepository propRepo)
    {
        _repo = repo;
        _propRepo = propRepo;
    }

    // GET: /Inmuebles
    public async Task<IActionResult> Index()
    {
        var list = await _repo.GetAllAsync();
        return View(list);
    }

    // GET: /Inmuebles/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var i = await _repo.GetByIdAsync(id);
        if (i == null) return NotFound();
        return View(i);
    }

    // GET: /Inmuebles/Create
    public async Task<IActionResult> Create2()
    {
        var propietarios = await _propRepo.GetAll();
        ViewBag.Propietarios = new SelectList(propietarios, "Id", "NombreCompleto");
        return View();
    }

    public async Task<IActionResult> Create()
{
    var propietarios = await _propRepo.GetAll();
    ViewBag.Propietarios = propietarios; 
    return View();
}


    // POST: /Inmuebles/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Inmueble i)
    {
        if (ModelState.IsValid)
        {
            await _repo.CreateAsync(i);
            return RedirectToAction(nameof(Index));
        }
        var propietarios = await _propRepo.GetAll();
        ViewBag.Propietarios = new SelectList(propietarios, "Id", "NombreCompleto", i.IdPropietario);
        return View(i);
    }

    // GET: /Inmuebles/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var i = await _repo.GetByIdAsync(id);
        if (i == null) return NotFound();
        var propietarios = await _propRepo.GetAll();
        ViewBag.Propietarios = new SelectList(propietarios, "Id", "NombreCompleto", i.IdPropietario);
        return View(i);
    }

    // POST: /Inmuebles/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Inmueble i)
    {
        if (id != i.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            await _repo.UpdateAsync(i);
            return RedirectToAction(nameof(Index));
        }
        var propietarios = await _propRepo.GetAll();
        ViewBag.Propietarios = new SelectList(propietarios, "Id", "NombreCompleto", i.IdPropietario);
        return View(i);
    }

    // GET: /Inmuebles/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var i = await _repo.GetByIdAsync(id);
        if (i == null) return NotFound();
        return View(i);
    }

    // POST: /Inmuebles/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repo.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}