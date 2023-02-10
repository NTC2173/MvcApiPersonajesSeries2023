using Microsoft.AspNetCore.Mvc;
using MvcApiPersonajesSeries2023.Models;
using MvcApiPersonajesSeries2023.Services;

namespace MvcApiPersonajesSeries2023.Controllers
{
    public class SeriesController : Controller
    {
        private ServiceSeries service;

        //Constructor: la función principal es inicializar los valores de las variables de instancia
        //de la clase y realizar cualquier otro procesamiento necesario para crear un objeto valido 
        //y listo para su uso
        public SeriesController(ServiceSeries service)
        {
            this.service = service;
        }
        
        public async Task<IActionResult> Index()
        {
            List<Serie> series = await this.service.GetSeriesAsync();
            return View(series);
        }

        public async Task<IActionResult> Details(int idserie)
        {
            Serie serie = await this.service.FindSerieAsync(idserie);
            return View(serie);
        }
    }
}
