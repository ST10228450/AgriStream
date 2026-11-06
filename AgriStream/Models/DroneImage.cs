
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriStream.Models
{
    public class DroneImage
    {
        [Key]
        public int DroneImageId { get; set; }

        [Required]
        public int FarmId { get; set; }

        // A drone image may be associated with a specific field
        public int? FieldId { get; set; }

        [Required]
        public int UploadedByUserId { get; set; }

        [Required]
        [StringLength(500)]
        public string BlobName { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string OriginalFileName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ContentType { get; set; } = string.Empty;

        public DateTime UploadedAtUtc { get; set; }
            = DateTime.UtcNow;

        [Required]
        [StringLength(30)]
        public string ProcessingStatus { get; set; } = "Pending";

        [ForeignKey(nameof(FarmId))]
        public Farm Farm { get; set; } = null!;

        [ForeignKey(nameof(FieldId))]
        public Field? Field { get; set; }

        [ForeignKey(nameof(UploadedByUserId))]
        public User UploadedByUser { get; set; } = null!;
    }
}
