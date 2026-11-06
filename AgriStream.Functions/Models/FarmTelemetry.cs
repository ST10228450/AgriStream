using Azure;
using Azure.Data.Tables;

namespace AgriStream.Functions.Models
{
    public class FarmTelemetry : ITableEntity
    {
        public string PartitionKey { get; set; } = "";
        public string RowKey { get; set; } = "";

        public DateTimeOffset? Timestamp { get; set; }
        public ETag ETag { get; set; }

        public string FarmId { get; set; } = "";
        public double SoilMoisture { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }
}