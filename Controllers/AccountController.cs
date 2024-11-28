using Backend_Api.dtos;
using Backend_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Backend_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase

    {
        private readonly UserManager<AppUser> _userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IConfiguration _configuration;


        public AccountController(UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }
        // api/account/register

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new AppUser
            {
                Email = registerDto.Email,
                NombreCompleto = registerDto.Nombre_Completo,
                UserName = registerDto.Email
            };

            var result = await _userManager.CreateAsync(user, registerDto.password);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            if (registerDto.roles is null)
            {
                await _userManager.AddToRoleAsync(user, "User");
            }
            else
            {
                foreach (var role in registerDto.roles)
                {
                    await _userManager.AddToRoleAsync(user, role);
                }
            }


            return Ok(new AuthResponseDto
            {
                IsSuccess = true,
                Message = "La cuenta fue creada con exito!"
            });

        }

        //api/account/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByEmailAsync(loginDto.Email);

            if (user is null)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Usuario no encontrado con este email",
                });
            }

            var result = await _userManager.CheckPasswordAsync(user, loginDto.Password);

            if (!result)
            {
                return Unauthorized(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Contraseña inválida."
                });
            }


            var token = GenerateToken(user);

            return Ok(new AuthResponseDto
            {
                Token = token,
                IsSuccess = true,
                Message = "Inicio de sesión exitoso."
            });


        }

        private string GenerateToken(AppUser user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var key = Encoding.ASCII
            .GetBytes(_configuration.GetSection("JWTSetting").GetSection("securityKey").Value!);

            var roles = _userManager.GetRolesAsync(user).Result;

            List<Claim> claims =
            [
                new (JwtRegisteredClaimNames.Email,user.Email??""),
                new (JwtRegisteredClaimNames.Name,user.NombreCompleto??""),
                new (JwtRegisteredClaimNames.NameId,user.Id ??""),
                new (JwtRegisteredClaimNames.Aud,
                _configuration.GetSection("JWTSetting").GetSection("validAudience").Value!),
                new (JwtRegisteredClaimNames.Iss,_configuration.GetSection("JWTSetting").GetSection("validIssuer").Value!)
            ];


            foreach (var role in roles)

            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256
                )
            };


            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);


        }

       
        //api/account/detail
        [HttpGet("detail")]
        public async Task<ActionResult<UserDetailDto>> GetUserDetail()
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userManager.FindByIdAsync(currentUserId!);


            if (user is null)
            {
                return NotFound(new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Usuario no encontrado"
                });
            }

            return Ok(new UserDetailDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.NombreCompleto,
                Roles = [.. await _userManager.GetRolesAsync(user)],
                PhoneNumber = user.PhoneNumber,
                PhoneNumberConfirmed = user.PhoneNumberConfirmed,
                AccessFailedCount = user.AccessFailedCount,

            });

        }

        //Esto es si tenemos pocos usuarios,
        //ya  que si tenemos demasiados el proceso sera lento (secuencial)

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers()
        {
            // Primero, obtenemos todos los usuarios de forma asíncrona
            var users = await _userManager.Users.ToListAsync();

            // creamos la lista de UserDetailDto de forma asíncrona
            var userDtos = new List<UserDetailDto>();

            foreach (var user in users)
            {
                // Obtenemos los roles de cada usuario de forma asíncrona
                var roles = await _userManager.GetRolesAsync(user);

                // Creamos el DTO para cada usuario
                var userDto = new UserDetailDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.NombreCompleto,
                    Roles = roles.ToArray()  // Convertimos a array
                };

                userDtos.Add(userDto);
            }

            return Ok(userDtos);  // Devolvemos la lista completa
        }
    }
}

/* Esto si tenemos demasidos usuarios, 
        * las solicitudes para obtener los roles se ejecutan de manera simultánea y no en secuencia. (parelelo)
       [HttpGet]
       public async Task<ActionResult<IEnumerable<UserDetailDto>>> GetUsers()
       {
           // Primero, obtenemos todos los usuarios de forma asíncrona
           var users = await _userManager.Users.ToListAsync();

           // Usamos Task.WhenAll para obtener los roles de todos los usuarios en paralelo
           var userDtos = await Task.WhenAll(users.Select(async user =>
           {
               var roles = await _userManager.GetRolesAsync(user);
               return new UserDetailDto
               {
                   Id = user.Id,
                   Email = user.Email,
                   FullName = user.NombreCompleto,
                   Roles = roles.ToArray()
               };
           }));

           return Ok(userDtos);
       }*/




