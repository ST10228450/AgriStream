
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriStream.Models
{
    public class IrrigationCommand
    {
        [Key]
        public int IrrigationCommandId { get; set; }

        [Required]
        public int FarmId { get; set; }

        [Required]
        public int FieldId { get; set; }

        [Required]
        public int RequestedByUserId { get; set; }

        [Range(1, 120)]
        public int DurationMinutes { get; set; }

        [Required]
        [StringLength(30)]
        public string CommandStatus { get; set; } = "Pending";

        public DateTime CreatedAtUtc { get; set; }
            = DateTime.UtcNow;

        public DateTime? ProcessedAtUtc { get; set; }

        [StringLength(100)]
        public string? QueueMessageId { get; set; }

        [StringLength(1000)]
        public string? FailureReason { get; set; }

        [ForeignKey(nameof(FarmId))]
        public Farm Farm { get; set; } = null!;

        [ForeignKey(nameof(FieldId))]
        public Field Field { get; set; } = null!;

        [ForeignKey(nameof(RequestedByUserId))]
        public User RequestedByUser { get; set; } = null!;
    }
}
