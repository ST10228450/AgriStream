namespace AgriStream.Services;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;



    public class BlobStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(BlobServiceClient client)
        {
            _container = client.GetBlobContainerClient("drone-images");
            _container.CreateIfNotExists(
                PublicAccessType.None);
        }

        public async Task<string> UploadAsync(
            IFormFile file,
            CancellationToken cancellationToken = default)
        {
            string extension =
                Path.GetExtension(file.FileName).ToLowerInvariant();

            string[] allowedExtensions =
                { ".jpg", ".jpeg", ".png", ".webp" };

            if (!allowedExtensions.Contains(extension))
                throw new InvalidOperationException(
                    "Only JPG, PNG and WEBP images are allowed.");

            if (file.Length == 0 || file.Length > 10 * 1024 * 1024)
                throw new InvalidOperationException(
                    "Image must be between 1 byte and 10 MB.");

            string blobName =
                $"{Guid.NewGuid()}{extension}";

            BlobClient blob = _container.GetBlobClient(blobName);

            await blob.UploadAsync(
                file.OpenReadStream(),
                new BlobUploadOptions
                {
                    HttpHeaders = new BlobHttpHeaders
                    {
                        ContentType = file.ContentType
                    }
                },
                cancellationToken);

            return blobName;
        }
    }


    
