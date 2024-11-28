using System.ComponentModel.DataAnnotations;

namespace Backend_Api.dtos
{
    public class CreateRoleDto
    {
        [Required(ErrorMessage = "El nombre del rol es obligatorio.")]
        public string RoleName { get; set; } = null!;
    }

}
