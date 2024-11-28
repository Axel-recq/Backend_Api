using System.ComponentModel.DataAnnotations;

namespace Backend_Api.dtos
{
    public class RegisterDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string Nombre_Completo { get; set; } = string.Empty;


        public string password { get; set; } = string.Empty;

        public List <string>? roles { get; set; }
    }
}
