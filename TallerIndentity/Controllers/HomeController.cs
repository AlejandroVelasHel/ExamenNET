using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TallerIndentity.Models;
using TallerIndentity.Models.ViewModels;

namespace GraficaSantiago.Controllers
{
    public class HomeController : Controller
    {
        private readonly TallerIdentityContext _dbcontext;

        public HomeController(TallerIdentityContext context)
        {
            _dbcontext = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult resumenVenta()
        {
            DateTime FechaInicio = DateTime.Now;
            FechaInicio = FechaInicio.AddDays(-5);
            var Lista = (from data in _dbcontext.Ventas.ToList()
                         group data by data.FechaVenta into gr
                         select new ViewVenta
                         {
                             cantidad = gr.Count(),
                             fecha = (DateTime)gr.Key
                         }
                         );
            return View(Lista);

        }

        public IActionResult resumenProducto()
        {
            List<ViewProducto> Lista = (from tbdetalleventa in _dbcontext.Productos.ToList()
                                             group tbdetalleventa by tbdetalleventa.Nombre into grupo
                                             orderby grupo.Count() descending
                                             select new ViewProducto
                                             {
                                                 producto = grupo.Key,
                                                 cantidad = grupo.Count(),

                                             }
                                            ).Take(4).ToList();
            //return View(Lista);
            return StatusCode(StatusCodes.Status200OK, Lista);
        }



        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}