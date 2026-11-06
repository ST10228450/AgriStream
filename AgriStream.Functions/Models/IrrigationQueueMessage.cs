namespace AgriStream.Functions.Models
{
    public class IrrigationQueueMessage
    {
        public int FarmId { get; set; }
        public int FieldId { get; set; }
        public int RequestedByUserId { get; set; }
        public int DurationMinutes { get; set; }
    }
}