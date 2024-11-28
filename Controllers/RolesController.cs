using Backend_Api.dtos;
using Backend_Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend_Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class RolesController:ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public RolesController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // crear un nuevo rol
        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleDto createRoleDto)
        {
            // Validar que el nombre del rol no esté vacío
            if (string.IsNullOrEmpty(createRoleDto.RoleName))
            {
                return BadRequest("El nombre del rol es obligatorio");
            }

            // Verificar si el rol ya existe
            var roleExist = await _roleManager.RoleExistsAsync(createRoleDto.RoleName);
            if (roleExist)
            {
                return BadRequest("El rol ya existe");
            }

            // Crear el nuevo rol
            var roleResult = await _roleManager.CreateAsync(new IdentityRole(createRoleDto.RoleName));

            // Verificar si la creación fue exitosa
            if (roleResult.Succeeded)
            {
                return Ok(new { message = "Rol creado exitosamente" });
            }

            // Si hubo un error al crear el rol, devolver un BadRequest con el mensaje de error
            return BadRequest("Error en la creación del rol.");
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RoleResponseDto>>> GetRoles()
        {
            // Obtener todos los roles (síncrono y seguro)
            var roles = await _roleManager.Roles.ToListAsync();

            var roleResponseList = new List<RoleResponseDto>();

            // Procesar cada rol de manera secuencial
            foreach (var role in roles)
            {
                // Obtener los usuarios para cada rol de forma secuencial
                var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

                // Crear el DTO para el rol con la cantidad de usuarios
                var roleResponse = new RoleResponseDto
                {
                    Id = role.Id,
                    Name = role.Name,
                    TotalUsers = usersInRole.Count
                };

                // Agregar el DTO de cada rol a la lista
                roleResponseList.Add(roleResponse);
            }

            // Devolver la lista completa de roles
            return Ok(roleResponseList);
        }






        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            //Encontrar el rol por su id

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
            {
                return NotFound("Rol no encontrado.");
            }

            var result = await _roleManager.DeleteAsync(role);

            if (result.Succeeded)
            {
                return Ok(new { message = "Rol eliminado exitosamente" });
            }

            return BadRequest("Error en la eliminación del rol.");

        }



        [HttpPost("assign")]
        //Tambien se podria implementar, si algun usuario tiene rol "user""Estilista" no se le pueda asignar un rol admin
        public async Task<IActionResult> AssignRole([FromBody] RoleAssignDto roleAssignDto)
        {
            var user = await _userManager.FindByIdAsync(roleAssignDto.UserId);

            if (user is null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Obtener el rol a través de su ID
            var role = await _roleManager.FindByIdAsync(roleAssignDto.RoleId);

            if (role == null)
            {
                return NotFound($"El rol con ID {roleAssignDto.RoleId} no existe.");
            }

            // Obtener los roles actuales del usuario
            var currentRoles = await _userManager.GetRolesAsync(user);

            // Si el usuario tiene el rol "User", lo eliminamos antes de asignar el nuevo rol
            if (currentRoles.Contains("User"))
            {
                await _userManager.RemoveFromRoleAsync(user, "User");
            }

            // Asignamos el nuevo rol usando el nombre del rol (role.Name)
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return Ok(new { message = "Rol asignado exitosamente." });
            }

            var error = result.Errors.FirstOrDefault();
            return BadRequest(error?.Description ?? "Error al asignar el rol.");
        }


        [HttpPost("remove")]
        public async Task<IActionResult> RemoveRole([FromBody] RoleAssignDto roleAssignDto)
        {
            // Buscar el usuario por su ID
            var user = await _userManager.FindByIdAsync(roleAssignDto.UserId);

            if (user is null)
            {
                return NotFound("Usuario no encontrado.");
            }

            // Buscar el rol por su ID
            var role = await _roleManager.FindByIdAsync(roleAssignDto.RoleId);

            if (role is null)
            {
                return NotFound("Rol no encontrado.");
            }

            // Eliminar el rol del usuario
            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            if (result.Succeeded)
            {
                return Ok(new { message = "Rol eliminado exitosamente del usuario." });
            }

            var error = result.Errors.FirstOrDefault();
            return BadRequest(error?.Description ?? "Error desconocido al eliminar el rol del usuario.");
        }

    }
}