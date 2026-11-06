
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AgriStream.Services;

namespace AgriStream.Controllers
{
    [Authorize]
    public class DroneImagesController : Controller
    {
        private readonly BlobStorageService _blobService;

        public DroneImagesController(
            BlobStorageService blobService)
        {
            _blobService = blobService;
        }

        // GET: DroneImages
        public IActionResult Index()
        {
            return View();
        }

        // GET: DroneImages/Upload
        [Authorize(Roles = "Farmer,Agronomist,Admin")]
        public IActionResult Upload()
        {
            return View();
        }

        // POST: DroneImages/Upload
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Farmer,Agronomist,Admin")]
        public async Task<IActionResult> Upload(
            IFormFile image,
            CancellationToken cancellationToken)
        {
            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError(
                    "image",
                    "Please select a drone image.");

                return View();
            }

            try
            {
                string blobName =
                    await _blobService.UploadAsync(
                        image, cancellationToken);

                TempData["Success"] =
                    "Drone image uploaded successfully.";

                TempData["BlobName"] = blobName;

                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);

                return View();
            }
            catch (Exception)
            {
                ModelState.AddModelError(
                    "",
                    "An error occurred while uploading the image.");

                return View();
            }
        }
    }
}
