using MvcApiPersonajesSeries2023.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcApiPersonajesSeries2023.Services
{
    public class ServiceSeries
    {
        private string UrlApi;

        //Propiedad privada llamada "header" de tipo
        //MediaTypeWithQualityHeaderValue. Esta propiedad se inicializa con un
        //valor de "application/json", lo que significa que los datos intercambiados
        //con la API serán en formato JSON
        private MediaTypeWithQualityHeaderValue header;

        public ServiceSeries(string url)
        {
            this.UrlApi = url;
            this.header = new MediaTypeWithQualityHeaderValue("application/json");
        }

        //Metodo privado llamado 'CallApiAsync', este metodo retorna un objeto de cualquier tipo
        //generico 'T' y recibe un parametro string llamado 'request'. 
        //Este metodo realiza una llamada asíncrona a una API externa usando el objeto HttpClient. 
        private async Task<T> CallApiAsync<T>(string request)
        {
            //Se crea una instancia de HttpClient dentro de un bloque 'using', significa que cada vez 
            //que se ejecute el codigo dentro del bloque, se libera automaticamente el recurso HttpClient
            using (HttpClient client = new HttpClient())
            {
                //Establece la direccion base para el cliente HttpClient en la direccion especificada 
                //en la propiedad UrlApi. 
                client.BaseAddress = new Uri(this.UrlApi);
                //Luego, se limpian los encabezados de la solicitud y se agrega el encabezado de tipo
                //de contenido 'application/json' establecido en la propiedad 'header'
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.header);
                //Despues se realiza una llamada asincrona a la API usando el metodo 'GetAsync' y pasa
                //el parametro 'request'. La respuesta recibida se almacena en una variable llamada
                //'response'
                HttpResponseMessage response =
                    await client.GetAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    //Si la respuesta API es exitosa se utiliza el metodo ReadAsAsync para deserializar 
                    //el contenido de la respuesta en un objeto del tipo especificado por 'T' y se retorna
                    //ese objeto
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    //Si la respuesta del API no es exitosa, se retorna el valor predeterminado para 'T'
                    return default(T);
                }
                //EXPLICACION: El tipo genérico "T" es un tipo de parámetro de tipo en C# que permite crear métodos,
                //clases, interfaces, estructuras, delegates, etc. que pueden trabajar con diferentes tipos de datos
                //sin tener que especificar un tipo concreto.
                //En este caso, el método "CallApiAsync" utiliza un tipo genérico "T" para que pueda devolver un
                //objeto de cualquier tipo, dependiendo de los datos recibidos de la respuesta de la API.
                //Esto permite que el método sea reutilizable y que pueda trabajar con diferentes tipos de datos
                //sin tener que crear una versión diferente del método para cada tipo de datos que se espera recibir
                //de la API
            }
        }

        #region SERIES

        //Metodo publico publico llamado 'GetSeriesAsync'. Este metodo devuelve una tarea que, 
        //cuando se complete, retornara una lista de objetos 'Serie'
        public async Task<List<Serie>> GetSeriesAsync()
        {
            //Define una cadena que contiene la ruta de la API
            string request = "/api/series";

            //LLama al metodo 'CallApiAsync' y se pasa la cadena 'request' y el tipo generico 
            //'List<Serie>'. La tarea devuelta por 'CallApiAsync' se asigna a una variable llamada
            //'series'
            List<Serie> series = await this.CallApiAsync<List<Serie>>(request);
            //Cuando se llame permite obtener una lista de objetos 'Serie' de la API, sin tener que 
            //preocuparse por los detalles de la llamada a la API o de la deserializacion de los datos
            //recibidos
            return series;
        }

        public async Task<Serie> FindSerieAsync(int idserie)
        {
            string request = "/api/series/" + idserie;
            Serie serie = await this.CallApiAsync<Serie>(request);
            return serie;
        }

        #endregion

        #region PERSONAJES

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            string request = "/api/personajes";
            List<Personaje> personajes = await this.CallApiAsync<List<Personaje>>(request);
            return personajes;
        }

        public async Task<List<Personaje>> GetPersonajesSerieAsync(int idserie)
        {
            string request = "/api/series/personajesserie/" + idserie;
            List<Personaje> personajes = await this.CallApiAsync<List<Personaje>>(request);
            return personajes;
        }

        public async Task<Personaje> FindPersonajeAsync(int idpersonaje)
        {
            string request = "/api/personajes/" + idpersonaje;
            Personaje personaje = await this.CallApiAsync<Personaje>(request);
            return personaje;
        }

        //Este metodo es una tarea que crea un nuevo personaje API
        public async Task CreatePersonajeAsync (int id, string nombre, string imagen, int idserie)
        {
            //Comienza creando un objeto HttpClient
            using (HttpClient client = new HttpClient())
            {
                //Se crea un nuevo objeto 'Personaje' y se asignan los valores para los parametros
                //'id', 'nombre', 'imagen', 'idserie' a las propiedades correspondientes del objeto
                //'Personaje'
                string request = "/api/personajes";
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                Personaje personaje = new Personaje();
                personaje.IdPersonaje = id;
                personaje.Nombre = nombre;
                personaje.Imagen = imagen;
                personaje.IdSerie = idserie;

                //Se serializa el objeto "Personaje" a una cadena JSON utilizando el método
                //"SerializeObject" de la clase JsonConvert.
                string json = JsonConvert.SerializeObject(personaje);
                // Se crea un nuevo objeto StringContent y se le pasa la cadena JSON serializada,
                // la codificación UTF-8 y el tipo de contenido "application/json"
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");
                //Finalmente, se llama al método "PostAsync" del objeto HttpClient y se le pasa la
                //cadena "request" y el objeto StringContent. Este método envía una solicitud
                //POST a la API con los datos del personaje serializados y crea un nuevo personaje
                //en la API.
                await client.PostAsync(request, content);
            }
        }

        public async Task UpdateSeriePersonajeAsync(int idpersonaje, int idserie)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "/api/personajes/updatepersonajeserie/"
                    + idpersonaje + "/" + idserie;
                client.BaseAddress = new Uri(this.UrlApi);
                client.DefaultRequestHeaders.Clear();
                //AUNQUE NO CREEMOS NADA, DEBEMOS ENVIAR UN OBJETO
                //VACIO Content EN LA PETICION DEL PUT
                StringContent content =
                    new StringContent("", Encoding.UTF8, "application/json");
                await client.PutAsync(request, content);
            }
        }

        #endregion
    }
}
