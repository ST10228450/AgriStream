using System.Data;

namespace AgriStream.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public int RoleID { get; set; }
        public Role Role { get; set; } = null!;
    }

}
