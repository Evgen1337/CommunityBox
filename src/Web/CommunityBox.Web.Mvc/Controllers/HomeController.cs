using System.Diagnostics;
using CommunityBox.Web.Mvc.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace CommunityBox.Web.Mvc.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}