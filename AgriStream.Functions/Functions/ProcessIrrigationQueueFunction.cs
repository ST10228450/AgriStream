using AgriStream.Functions.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace AgriStream.Functions.Functions
{
    public class ProcessIrrigationQueueFunction
    {
        private readonly ILogger<ProcessIrrigationQueueFunction> _logger;

        public ProcessIrrigationQueueFunction(
            ILogger<ProcessIrrigationQueueFunction> logger)
        {
            _logger = logger;
        }

        [Function("ProcessIrrigationQueueFunction")]
        public async Task Run(
            [QueueTrigger(
                "irrigation-commands",
                Connection = "AzureWebJobsStorage")]
            string queueMessage)
        {
            var command =
                JsonSerializer.Deserialize<IrrigationQueueMessage>(
                    queueMessage);

            if (command == null)
            {
                _logger.LogError(
                    "Invalid irrigation queue message received.");

                return;
            }

            _logger.LogInformation(
                "Processing irrigation command. Farm: {FarmId}, Field: {FieldId}, Duration: {Duration} minutes.",
                command.FarmId,
                command.FieldId,
                command.DurationMinutes);

            // Simulated irrigation processing.
            // Replace this section with actual device integration
            // when irrigation hardware is available.

            await Task.CompletedTask;

            _logger.LogInformation(
                "Irrigation command processed successfully for Farm {FarmId}.",
                command.FarmId);
        }
    }
}