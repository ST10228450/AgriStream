
using AgriStream.Models;
using Azure.Data.Tables;

namespace AgriStream.Services
{
    public class TelemetryStorageService
    {
        private readonly TableClient _table;

        public TelemetryStorageService(
            TableServiceClient client)
        {
            _table = client.GetTableClient("FarmTelemetry");
            _table.CreateIfNotExists();
        }

        public async Task SaveTelemetryAsync(
            FarmTelemetry telemetry)
        {
            telemetry.PartitionKey = telemetry.FarmId;

            telemetry.RowKey =
                $"{DateTime.UtcNow.Ticks:D19}_{Guid.NewGuid():N}";

            await _table.AddEntityAsync(telemetry);
        }

        public async Task<List<FarmTelemetry>> GetFarmTelemetryAsync(
            string farmId)
        {
            var results = new List<FarmTelemetry>();

            await foreach (var item in _table.QueryAsync<FarmTelemetry>(
                x => x.PartitionKey == farmId))
            {
                results.Add(item);
            }

            return results;
        }
    }
}
