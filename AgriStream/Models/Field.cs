
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriStream.Models
{
    public class Field
    {
        [Key]
        public int FieldId { get; set; }

        [Required]
        public int FarmId { get; set; }

        [Required]
        [StringLength(100)]
        public string FieldName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string CropType { get; set; } = string.Empty;

        [Column(TypeName = "decimal(10,2)")]
        public decimal AreaHectares { get; set; }

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "Active";

        public DateTime CreatedAtUtc { get; set; }
            = DateTime.UtcNow;

        [ForeignKey(nameof(FarmId))]
        public Farm Farm { get; set; } = null!;

        public ICollection<DroneImage> DroneImages { get; set; }
            = new List<DroneImage>();

        public ICollection<IrrigationCommand> IrrigationCommands
        { get; set; } = new List<IrrigationCommand>();
    }
}
