using System.Diagnostics;
using foro_C.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace foro_C.Controllers
{
    public class HomeController : Controller
    {


        public ActionResult Index()
        {
            var resultadoVista = View();
            //retornaria un archivo html .el index
            return resultadoVista;
        }

        public ActionResult Privacy()
        {
            var resultadoVista = View();
            //retornaria el archivo privacy de html
            return resultadoVista;
        }


    }
}
