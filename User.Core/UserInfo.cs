using System;
using System.ComponentModel.DataAnnotations;

namespace User.Core.Model
{
    public class UserInfo
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string? FullName { get; set; }

        [Range(0, 150)]
        public int Age { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string? Password { get; set; }

        public string? RefreshToken { get; set; }

        public DateTime? RefreshTokenEndDate { get; set; }

        [Required]
        public string? Role { get; set; }
    }
}
