using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repositories.Interfaces;

namespace MVC.Controllers
{
    [Route("[controller]")]
    public class ContactController : Controller
    {
        private readonly IContactInterface _contact;
        private readonly ILogger<ContactController> _logger;

        public ContactController(IContactInterface contact, ILogger<ContactController> logger)
        {
            _contact = contact;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("kendoContact")]
        public IActionResult kendoContact()
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