
using AgriStream.Services;
using Microsoft.AspNetCore.Mvc;


namespace AgriStream.Controllers
{
    public class MediaController : Controller
    {
        private readonly BlobStorageService _blobService;

        public MediaController(BlobStorageService blobService)
        {
            _blobService = blobService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(
            IFormFile image,
            CancellationToken cancellationToken)
        {
            if (image == null || image.Length == 0)
            {
                ModelState.AddModelError(
                    "", "Please select an image.");

                return View();
            }

            try
            {
                string blobName = await _blobService.UploadAsync(
                    image, cancellationToken);

                // Save blobName and associated farmer/farm ID
                // in Azure SQL Database here.

                TempData["Success"] =
                    "Image uploaded successfully.";

                return RedirectToAction("Index");
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }
    }
}
