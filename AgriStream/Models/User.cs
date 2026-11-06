
namespace AgriStream.Models
{
    public class User
    {
        public int UserID { get; set; }

        public string Username { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;

        public int RoleID { get; set; }

        public Role Role { get; set; } = null!;

        public ICollection<Farm> Farms { get; set; }
            = new List<Farm>();

        public ICollection<DroneImage> UploadedDroneImages
        { get; set; } = new List<DroneImage>();

        public ICollection<IrrigationCommand> IrrigationCommands
        { get; set; } = new List<IrrigationCommand>();
    }
}
