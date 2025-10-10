using inmobilariaCeli.Models;
using inmobilariaCeli.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace inmobilariaCeli.Controllers
{
    public class PagosController : Controller
    {
        private readonly PagoRepository _repo;
        private readonly ContratoRepository _contratoRepo;

        public PagosController(PagoRepository repo, ContratoRepository contratoRepo)
        {
            _repo = repo;
            _contratoRepo = contratoRepo;
        }

        // 🔹 Listar pagos por contrato
        public async Task<IActionResult> Index(int contratoId)
        {
            var pagos = await _repo.GetByContratoIdAsync(contratoId);

            // ✅ Asegura que la vista reciba el contratoId
            ViewBag.ContratoId = contratoId;

            return View(pagos);
        }

        // 🔹 Mostrar formulario para crear pago
        public async Task<IActionResult> Create(int contratoId)
        {
            if (contratoId <= 0)
            {
                TempData["Error"] = "ID de contrato inválido.";
                return RedirectToAction("Index", new { contratoId = 0 });
            }

            var cantidadPagos = await _repo.ContarPagosPorContratoAsync(contratoId);
            var contrato = await _contratoRepo.GetByIdAsync(contratoId);

            if (contrato == null)
            {
                TempData["Error"] = $"Contrato con ID {contratoId} no encontrado.";
                return RedirectToAction("Index", new { contratoId });
            }

            // ✅ Validación: no permitir más pagos que los períodos definidos
            if (cantidadPagos >= contrato.Cuotas)
            {
                TempData["Error"] = "Este contrato ya tiene todos los pagos registrados.";
                return RedirectToAction("Index", new { contratoId });
            }

            var pago = new Pago
            {
                IdContrato = contratoId,
                NumeroPago = cantidadPagos + 1,
                FechaPago = DateTime.Now,
                Importe = contrato.MontoMensual
            };

            ViewBag.MontoContrato = contrato.MontoMensual;
            ViewBag.NumeroPago = pago.NumeroPago;

            return View(pago);
        }

        // 🔹 Guardar nuevo pago
        [HttpPost]
        public async Task<IActionResult> Create(Pago pago)
        {
            if (!ModelState.IsValid)
                return View(pago);

            var contrato = await _contratoRepo.GetByIdAsync(pago.IdContrato);
            if (contrato == null)
            {
                TempData["Error"] = $"Contrato con ID {pago.IdContrato} no encontrado.";
                return RedirectToAction("Index", new { contratoId = pago.IdContrato });
            }

            var cantidadPagos = await _repo.ContarPagosPorContratoAsync(pago.IdContrato);
            if (cantidadPagos >= contrato.Cuotas)
            {
                TempData["Error"] = "No se pueden registrar más pagos para este contrato.";
                return RedirectToAction("Index", new { contratoId = pago.IdContrato });
            }

            await _repo.CreateAsync(pago);
            return RedirectToAction("Index", new { contratoId = pago.IdContrato });
        }

        // 🔹 Marcar pago como abonado
        public async Task<IActionResult> Abonar(int id, int contratoId)
        {
            await _repo.SetPagadoAsync(id);
            return RedirectToAction("Index", new { contratoId });
        }

        // 🔹 Eliminar pago
        public async Task<IActionResult> Delete(int id, int contratoId)
        {
            await _repo.DeleteAsync(id);
            return RedirectToAction("Index", new { contratoId });
        }
    }
}