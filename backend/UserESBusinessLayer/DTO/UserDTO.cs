using System.ComponentModel.DataAnnotations;

namespace UserESBusinessLayer.DTO
{
    public class UserDTO
    {
        public int IdUsuario { get; set; }
        [Required]
        public required string NombreUsuario { get; set; }
        [Required]
        public required string Email { get; set; }
        
        public string? UsrPassword { get; set; }
    }
}
