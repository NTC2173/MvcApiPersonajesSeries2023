using Microsoft.AspNetCore.Mvc;
using MvcApiPersonajesSeries2023.Helpers;
using MvcApiPersonajesSeries2023.Models;
using MvcApiPersonajesSeries2023.Services;

namespace MvcApiPersonajesSeries2023.Controllers
{
    public class PersonajesController : Controller
    {
        private ServiceSeries service;
        private HelperPathProvider helperPath;
       

        public PersonajesController(ServiceSeries service
            , HelperPathProvider helperPath)
        {
            this.service = service;
            this.helperPath = helperPath;
            
        }

        public async Task<IActionResult> PersonajesSerie(int idserie)
        {
            List<Personaje> personajes =
                await this.service.GetPersonajesSerieAsync(idserie);
            return View(personajes);
        }

        //Metodo HTTP GET que se utiliza para mostrar una una vista donde se puede crear un nuevo
        //personaje.
        public async Task<IActionResult> CreatePersonaje()
        {
            List<Serie> series = await this.service.GetSeriesAsync();
            ViewData["SERIES"] = series;
            return View();
        }

                //Metodo HTTP POST para crear un nuevo personaje. Toma por objeto 'Personaje' como
        //parametro.
        [HttpPost]
        public async Task<IActionResult> CreatePersonaje
            (Personaje personaje, IFormFile fichero)
        {
            //DEBEMOS SUBIR EL FICHERO AL SERVIDOR AZURE
            string fileName = fichero.FileName;

            //Hace una llamada en el servicio proporcionado para crear un nuevo personaje
            //en la base de datos
            await this.service.CreatePersonajeAsync(personaje.IdPersonaje
                , personaje.Nombre, personaje.Imagen, personaje.IdSerie);
            //LO VAMOS A LLEVAR A LA VISTA PERSONAJES SERIE
            return RedirectToAction("PersonajesSerie", new { idserie = personaje.IdSerie });
        }

        //Este metodo es una accion HTTP GET que recupera una lista de personajes y series
        //utilizando el metodo 'GetPersonajesAsync' y 'GetSeriesAsync' del servicio y 
        //almacena estos datos en el 'ViewData' para su uso posterior en la vista 
        public async Task<IActionResult> UpdatePersonajeSerie()
        {
            List<Personaje> personajes = await this.service.GetPersonajesAsync();
            List<Serie> series = await this.service.GetSeriesAsync();
            ViewData["PERSONAJES"] = personajes;
            ViewData["SERIES"] = series;
            return View();
        }

        //El metodo HTTP POST es una accion que se invoca cuando se envía la solicitud 
        //HTTP POST al controlador. Toma como argumentos los 'idpersonaje' e 'ideserie'
        [HttpPost]
        public async Task<IActionResult> UpdatePersonajeSerie
            (int idpersonaje, int idserie)
        {
            //Utiliza el metodo 'UpdateSeriePersonajeAsync' del servicio para actualizar
            //la serie de un personaje especifico
            await this.service.UpdateSeriePersonajeAsync(idpersonaje, idserie);
            //Finalmente redirige  al usuario a la accion 'PersonajesSerie' y le pasa
            //el 'idserie' como argumento
            return RedirectToAction("PersonajesSerie"
                , new { idserie = idserie });
        }
    }
}
