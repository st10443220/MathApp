using System.ComponentModel.DataAnnotations;

namespace MathApp.Models
{
    public class ProfileViewModel
    {
        public string? Email { get; set; }

        [Required(ErrorMessage = "You gotta have a username, bro.")]
        [StringLength(
            20,
            MinimumLength = 3,
            ErrorMessage = "Username must be between 3 and 20 characters."
        )]
        public string Username { get; set; } = string.Empty;
    }
}
