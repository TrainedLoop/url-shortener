using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using UrlShortener.Models;
using UrlShortener.Repository.Contract;

namespace UrlShortener.Controllers
{
    public class HomeController : Controller
    {
        public IUrlShortedRepository Repository { get; }
        public IConfiguration Configuation { get; }

        public HomeController(IUrlShortedRepository repository, IConfiguration configuation)
        {
            Repository = repository;
            Configuation = configuation;
        }


        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(Models.UrlShortedModel model)
        {
            if (ModelState.IsValid)
            {
                string hashCode = string.Format("{0:X}", Guid.NewGuid().ToString().GetHashCode());

                var urlShortd = new UrlShortener.Repository.Model.UrlShorted()
                {
                    Hash = await GenerateNewHashAsync(),
                    RealUrl = model.Url.ToString(),
                    ExpiresIn = DateTime.Now.AddDays(
                            int.Parse(Configuation.GetSection("UrlShortener:ExpirationDays").Value))

                };
                await Repository.CreateAsync(urlShortd);
                model.ShortedHash = $"{Request.Scheme}://{Request.Host}/r/{urlShortd.Hash}";
                return View(model);

            }
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("r/{hash}")]
        public async Task<IActionResult> RedirectAsync(string hash)
        {
            var url = await Repository.GetHashAsync(hash);
            if (url == null)
                return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });

            await Repository.CountClick(hash);
            return Redirect(url.RealUrl);
        }

        private async Task<string> GenerateNewHashAsync()
        {
            string hashCode = string.Format("{0:X}", Guid.NewGuid().ToString().GetHashCode());
            var dbHash = await Repository.GetHashAsync(hashCode);
            if (dbHash != null)
            {
                if (dbHash.ExpiresIn < DateTime.Now)
                    await Repository.RemoveHash(hashCode);
                else
                {
                    return await GenerateNewHashAsync();
                }
                return hashCode;
            }
            return hashCode;

        }
    }
}
