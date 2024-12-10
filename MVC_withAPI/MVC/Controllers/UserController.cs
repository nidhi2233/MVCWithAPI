using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using repositories.Interfaces;
using repositories.Models;

namespace MVC.Controllers
{
    //[Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserInterface _userRepo;

        public UserController(IUserInterface _userRepo)
        {
            this._userRepo = _userRepo;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult kendoLogin()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Kendoregister()
        {
            return View();
        }


        private readonly ILogger<UserController> _logger;

        // public UserController(ILogger<UserController> logger)
        // {
        //     _logger = logger;
        // }

        public IActionResult Index()
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