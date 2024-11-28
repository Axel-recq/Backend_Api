using Backend_Api.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend_Api.Data
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext ( DbContextOptions<AppDbContext>options):base( options )
        {

        }
    }
}
