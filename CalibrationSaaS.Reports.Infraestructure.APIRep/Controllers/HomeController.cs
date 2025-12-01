using CalibrationSaaS.Reports.Infraestructure.APIRep.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using CalibrationSaaS.Infraestructure.EntityFramework;
using CalibrationSaaS.Domain.Aggregates.Entities;
using CalibrationSaaS.Infraestructure.EntityFramework.DataAccess;
using CalibrationSaaS.Application.UseCases;
using Microsoft.AspNetCore.Components;

namespace CalibrationSaaS.Reports.Infraestructure.APIRep.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly WorkOrderDetailUseCase wodLogic;
       
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }


        public IActionResult Index()
        {
          
          
            return View();
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