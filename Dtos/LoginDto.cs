using System.ComponentModel.DataAnnotations;

namespace sof1_IIPA26.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "El usuario es requerido")]
        public string NombreUsuario { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es requerida")]
        public string Contrasena { get; set; } = string.Empty;

        public int Respuesta { get; set; }
        public string Mensaje { get; set; } = string.Empty;
    }
}
