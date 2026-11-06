
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgriStream.Services;

namespace AgriStream.Controllers
{
    [Authorize]
    public class FieldsController : Controller
    {
        private readonly TelemetryStorageService _telemetryService;

        public FieldsController(
            TelemetryStorageService telemetryService)
        {
            _telemetryService = telemetryService;
        }

        // GET: Fields
        public IActionResult Index()
        {
            ViewBag.Title = "Field Monitoring";

            return View();
        }

        // GET: Fields/Details?farmId=FARM001
        public async Task<IActionResult> Details(
            string farmId)
        {
            if (string.IsNullOrWhiteSpace(farmId))
            {
                return BadRequest("Farm ID is required.");
            }

            var telemetry =
                await _telemetryService
                    .GetFarmTelemetryAsync(farmId);

            ViewBag.FarmId = farmId;

            return View(telemetry);
        }
    }
}
