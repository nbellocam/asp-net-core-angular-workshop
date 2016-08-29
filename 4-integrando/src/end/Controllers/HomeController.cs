using Microsoft.AspNetCore.Mvc;

namespace TourOfHeroes.Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }
  }
}