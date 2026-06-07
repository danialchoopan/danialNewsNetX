using Microsoft.AspNetCore.Mvc;

namespace danialNewsNetX.WebUI.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
