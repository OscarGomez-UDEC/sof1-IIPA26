using System.ComponentModel.DataAnnotations;

namespace sof1_IIPA26.Dtos
{
    public class UserDto
    {
        public int IdUser { get; set; }
        public int IdRole { get; set; }
        public string Role { get; set; } = string.Empty;
        public int IdState { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 100 caracteres")]
        public string Name { get; set; } = string.Empty;

        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "El usuario es requerido")]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "El usuario debe tener entre 4 y 15 caracteres")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        [StringLength(200, MinimumLength = 6, ErrorMessage = "La contraseña debe tener entre 6 y 200 caracteres")]
        public string Password { get; set; } = string.Empty;

        public int Response { get; set; }
        public string Message { get; set; } = string.Empty;
    }
}
