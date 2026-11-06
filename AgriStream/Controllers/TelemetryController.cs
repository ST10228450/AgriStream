
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgriStream.Services;

namespace AgriStream.Controllers
{
    [Authorize(Roles = "Agronomist,Admin")]
    public class TelemetryController : Controller
    {
        private readonly TelemetryStorageService _telemetryService;

        public TelemetryController(
            TelemetryStorageService telemetryService)
        {
            _telemetryService = telemetryService;
        }

        // GET: Telemetry?farmId=FARM001
        public async Task<IActionResult> Index(string farmId)
        {
            if (string.IsNullOrWhiteSpace(farmId))
            {
                ViewBag.Message =
                    "Select a farm to view telemetry.";

                return View();
            }

            var readings =
                await _telemetryService
                    .GetFarmTelemetryAsync(farmId);

            ViewBag.FarmId = farmId;

            return View(readings);
        }
    }
}
