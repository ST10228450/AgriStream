using Azure;
using Azure.Data.Tables;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace AgriStream.Functions.Functions
{
    public class DroneImageBlobFunction
    {
        private readonly TableServiceClient _tableService;
        private readonly ILogger<DroneImageBlobFunction> _logger;

        public DroneImageBlobFunction(
            TableServiceClient tableService,
            ILogger<DroneImageBlobFunction> logger)
        {
            _tableService = tableService;
            _logger = logger;
        }

        [Function("DroneImageBlobFunction")]
        public async Task Run(
            [BlobTrigger(
                "drone-images/{name}",
                Connection = "AzureWebJobsStorage")]
            Stream blob,
            string name)
        {
            var table = _tableService.GetTableClient(
                "DroneImageAudit");

            await table.CreateIfNotExistsAsync();

            var audit = new TableEntity(
                "DroneImage",
                Guid.NewGuid().ToString())
            {
                ["FileName"] = name,
                ["FileSize"] = blob.Length,
                ["ProcessingStatus"] = "Received",
                ["UploadedAtUtc"] = DateTime.UtcNow,
                ["Message"] = "Drone image detected by Blob Trigger"
            };

            await table.AddEntityAsync(audit);

            _logger.LogInformation(
                "Drone image {FileName} detected. Audit entry created.",
                name);
        }
    }
}