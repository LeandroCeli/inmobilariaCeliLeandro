using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace inmobilariaCeli.Controllers;

public class InquilinosController : Controller
{
    private readonly InquilinoRepository _repo;
    public InquilinosController(InquilinoRepository repo) => _repo = repo;

    // GET: /Inquilinos
    public async Task<IActionResult> Index()
    {
        var list = await _repo.GetAllAsync();
        return View(list);
    }

    // GET: /Inquilinos/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var i = await _repo.GetByIdAsync(id);
        if (i == null) return NotFound();
        return View(i);
    }

    // GET: /Inquilinos/Create
    public IActionResult Create() => View();

    // POST: /Inquilinos/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Inquilino i)
    {
        if (ModelState.IsValid)
        {
            await _repo.CreateAsync(i);
            return RedirectToAction(nameof(Index));
        }
        return View(i);
    }

    // GET: /Inquilinos/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var i = await _repo.GetByIdAsync(id);
        if (i == null) return NotFound();
        return View(i);
    }

    // POST: /Inquilinos/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Inquilino i)
    {
        if (id != i.Id) return BadRequest();
        if (ModelState.IsValid)
        {
            await _repo.UpdateAsync(i);
            return RedirectToAction(nameof(Index));
        }
        return View(i);
    }

    // GET: /Inquilinos/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var i = await _repo.GetByIdAsync(id);
        if (i == null) return NotFound();
        return View(i);
    }

    // POST: /Inquilinos/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repo.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}
