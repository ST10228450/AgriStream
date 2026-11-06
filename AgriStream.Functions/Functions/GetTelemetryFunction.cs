using Azure.Data.Tables;
using AgriStream.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;

namespace AgriStream.Functions.Functions
{
    public class GetTelemetryFunction
    {
        private readonly TableServiceClient _tableService;
        private readonly ILogger<GetTelemetryFunction> _logger;

        public GetTelemetryFunction(
            TableServiceClient tableService,
            ILogger<GetTelemetryFunction> logger)
        {
            _tableService = tableService;
            _logger = logger;
        }

        [Function("GetTelemetryFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "get",
                Route = "telemetry/{farmId}")]
            HttpRequestData request,
            string farmId)
        {
            var response = request.CreateResponse();

            try
            {
                var table = _tableService.GetTableClient(
                    "FarmTelemetry");

                var readings = new List<FarmTelemetry>();

                await foreach (var reading in table.QueryAsync<FarmTelemetry>(
                    filter: x => x.PartitionKey == farmId))
                {
                    readings.Add(reading);
                }

                var latestReadings = readings
                    .OrderByDescending(x => x.Timestamp)
                    .Take(20)
                    .ToList();

                response.StatusCode = HttpStatusCode.OK;

                await response.WriteAsJsonAsync(latestReadings);

                _logger.LogInformation(
                    "Retrieved {Count} telemetry records for farm {FarmId}.",
                    latestReadings.Count,
                    farmId);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to retrieve telemetry for farm {FarmId}.",
                    farmId);

                response.StatusCode =
                    HttpStatusCode.InternalServerError;

                await response.WriteStringAsync(
                    "Unable to retrieve farm telemetry.");

                return response;
            }
        }
    }
}