using MvcApiPersonajesSeries2023.Services;

namespace MvcApiPersonajesSeries2023
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //Se registra un servicio en la aplicacion que se utilizara para realizar llamadas a una 
            //API de series y personajes, y se esta utilizando la configuracion para especificar la URL
            //de la API
            string apiSeries = builder.Configuration.GetValue<string>("ApiUrls:ApiSeriesPersonajes");
            builder.Services.AddTransient<ServiceSeries>(z => new ServiceSeries(apiSeries));


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}