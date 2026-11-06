
using System.Net.Http.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AgriStream.Controllers
{
    [Authorize]
    public class IrrigationController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;

        public IrrigationController(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
        }

        // GET: Irrigation
        public IActionResult Index()
        {
            return View();
        }

        // POST: Irrigation/Trigger
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer,Agronomist,Admin")]
        public async Task<IActionResult> Trigger(
            string farmId,
            string fieldId,
            int durationMinutes)
        {
            if (string.IsNullOrWhiteSpace(farmId) ||
                string.IsNullOrWhiteSpace(fieldId) ||
                durationMinutes < 1 ||
                durationMinutes > 120)
            {
                TempData["Error"] =
                    "Please provide valid irrigation details.";

                return RedirectToAction(nameof(Index));
            }

            var functionUrl =
                _configuration["Functions:TriggerIrrigationUrl"];

            if (string.IsNullOrWhiteSpace(functionUrl))
            {
                TempData["Error"] =
                    "Irrigation Function URL is not configured.";

                return RedirectToAction(nameof(Index));
            }

            var command = new IrrigationRequest
            {
                FarmId = farmId,
                FieldId = fieldId,
                DurationMinutes = durationMinutes,
                RequestedBy = User.Identity?.Name ?? "Unknown"
            };

            try
            {
                var client = _httpClientFactory.CreateClient();

                var response = await client.PostAsJsonAsync(
                    functionUrl,
                    command);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Success"] =
                        "Irrigation command submitted successfully.";
                }
                else
                {
                    TempData["Error"] =
                        "The irrigation command could not be submitted.";
                }
            }
            catch (HttpRequestException)
            {
                TempData["Error"] =
                    "Could not connect to the irrigation service.";
            }

            return RedirectToAction(nameof(Index));
        }
    }

    public class IrrigationRequest
    {
        public string FarmId { get; set; } = string.Empty;

        public string FieldId { get; set; } = string.Empty;

        public int DurationMinutes { get; set; }

        public string RequestedBy { get; set; } = string.Empty;
    }
}
