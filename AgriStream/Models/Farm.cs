
using Apache.Arrow;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AgriStream.Models
{
    public class Farm
    {
        [Key]
        public int FarmId { get; set; }

        [Required]
        [StringLength(100)]
        public string FarmName { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string Location { get; set; } = string.Empty;

        // User who owns/manages the farm
        [Required]
        public int FarmerId { get; set; }

        [ForeignKey(nameof(FarmerId))]
        public User Farmer { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }
            = DateTime.UtcNow;

        public ICollection<Field> Fields { get; set; }
            = new List<Field>();

        public ICollection<DroneImage> DroneImages { get; set; }
            = new List<DroneImage>();

        public ICollection<IrrigationCommand> IrrigationCommands
        { get; set; } = new List<IrrigationCommand>();
    }
}
