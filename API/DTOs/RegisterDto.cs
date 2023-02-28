using System.ComponentModel.DataAnnotations; // [Required]

namespace API.DTOs {

    public class RegisterDto {

        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }

}