
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriStream.Controllers
{
    [Authorize]
    public class FarmsController : Controller
    {
        // GET: Farms
        public IActionResult Index()
        {
            ViewBag.Title = "Farm Management";
            ViewBag.Message =
                "Manage and monitor your registered farms.";

            return View();
        }

        // GET: Farms/Details/1
        public IActionResult Details(int id)
        {
            ViewBag.FarmId = id;

            return View();
        }

        // GET: Farms/Create
        [Authorize(Roles = "Farmer,Admin")]
        public IActionResult Create()
        {
            return View();
        }
    }
}


