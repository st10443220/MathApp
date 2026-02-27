using System.ComponentModel.DataAnnotations;

namespace MathApp.Models
{
    public class UserProfile
    {
        [Key]
        public int Id { get; set; }

        public string FirebaseUuid { get; set; } = string.Empty;

        public string? Username { get; set; }
    }
}
