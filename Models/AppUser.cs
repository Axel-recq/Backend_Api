using Microsoft.AspNetCore.Identity;

namespace Backend_Api.Models
{
    public class AppUser:IdentityUser
    {
        public string? NombreCompleto { get; set; }
        
    }
}
