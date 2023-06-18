using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Email_Scheduler.Models;
using Email_Scheduler.Interface;
using System.Net.Mail;
using System.Net;

namespace Email_Scheduler.Controllers
{
    public class HomeController : Controller, IJob
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMailService mailService;
        public HomeController(ILogger<HomeController> logger, IMailService mailService)
        {
            _logger = logger;
            this.mailService = mailService;
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

        public void SendMail()
        {
            //Console.WriteLine($"Hanfire recurring job!");
            try
            {
                mailService.SendEmail();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
