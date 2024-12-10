using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Repositories.Interfaces;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class EmployeeController : Controller
    {
        private readonly ILogger<EmployeeController> _logger;
        private readonly EmployeeInterface _empRepo;

        public EmployeeController(EmployeeInterface _empRepo, ILogger<EmployeeController> logger)
        {
            this._empRepo = _empRepo;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Dashboard()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}