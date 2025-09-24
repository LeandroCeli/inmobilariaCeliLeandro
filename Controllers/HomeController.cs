using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using inmobilariaCeli.Models;

namespace inmobilariaCeli.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        // Constructor con inyección de dependencias para el logger
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Acción principal: vista de inicio
        public IActionResult Index()
        {
            return View();
        }

        // Acción para la vista de privacidad
        public IActionResult Privacy()
        {
            return View();
        }

        // Acción para la vista "Acerca de"
        public IActionResult About()
        {
            return View();
        }

        // Acción para manejar errores
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel 
            { 
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier 
            });
        }
    }
}