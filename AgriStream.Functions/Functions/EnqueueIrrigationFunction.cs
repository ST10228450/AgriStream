using Azure.Storage.Queues;
using AgriStream.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Text.Json;

namespace AgriStream.Functions.Functions
{
    public class EnqueueIrrigationFunction
    {
        private readonly QueueClient _queueClient;
        private readonly ILogger<EnqueueIrrigationFunction> _logger;

        public EnqueueIrrigationFunction(
            QueueClient queueClient,
            ILogger<EnqueueIrrigationFunction> logger)
        {
            _queueClient = queueClient;
            _logger = logger;
        }

        [Function("EnqueueIrrigationFunction")]
        public async Task<HttpResponseData> Run(
            [HttpTrigger(
                AuthorizationLevel.Function,
                "post",
                Route = "irrigation")]
            HttpRequestData request)
        {
            var response = request.CreateResponse();

            try
            {
                var command =
                    await request.ReadFromJsonAsync<IrrigationQueueMessage>();

                if (command == null ||
                    command.FarmId <= 0 ||
                    command.FieldId <= 0 ||
                    command.RequestedByUserId <= 0 ||
                    command.DurationMinutes < 1 ||
                    command.DurationMinutes > 120)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;

                    await response.WriteStringAsync(
                        "Invalid irrigation request.");

                    return response;
                }

                string message = JsonSerializer.Serialize(command);

                await _queueClient.CreateIfNotExistsAsync();

                await _queueClient.SendMessageAsync(message);

                response.StatusCode = HttpStatusCode.Accepted;

                await response.WriteAsJsonAsync(new
                {
                    Message = "Irrigation command successfully queued.",
                    command.FarmId,
                    command.FieldId,
                    command.DurationMinutes
                });

                _logger.LogInformation(
                    "Irrigation command queued for farm {FarmId}.",
                    command.FarmId);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    "Failed to enqueue irrigation command.");

                response.StatusCode =
                    HttpStatusCode.InternalServerError;

                await response.WriteStringAsync(
                    "Unable to enqueue irrigation command.");

                return response;
            }
        }
    }
}