using sof1_IIPA26.Dtos;
using sof1_IIPA26.Repositories;
using sof1_IIPA26.Utilities;

namespace sof1_IIPA26.Services
{
    public class UserService
    {
        private readonly UserRepository _userRepository;

        public UserService(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<UserDto> CreateUser(UserDto userDto)
        {
            //Validacion de usuario
            userDto.Password = Encrypt.GetSHA256(userDto.Password);
            userDto.IdRole = 2;
            userDto.IdState = 1;
            var result = await _userRepository.CreateUser(userDto);
            if (result)
            {
                userDto.Response = 1;
                userDto.Message = "Ususario creado exitosamente.";
            }
            else
            {
                userDto.Response = 0;
                userDto.Message = "Usuario ya esxiste.";
            }

            return userDto;
        }

        public async Task<LoginDto> InicioSesion(LoginDto loginDto)
        {
            loginDto.Contrasena = Encrypt.GetSHA256(loginDto.Contrasena);

            var userResult = await _userRepository.Login(loginDto);

            if (userResult)
            {
                loginDto.Respuesta = 1;
                loginDto.Mensaje = "Inicio de Sesion Exitoso.";         
            }
            else
            {
                loginDto.Respuesta = 0;
                loginDto.Mensaje = "Inicio de Sesion Fallido, Usuario y/o Contraeña estan incorrectos.";
            }

            return loginDto;
        }

        public async Task<List<UserDto>> ObtenerUsuarios()
        {
            return await _userRepository.ObtenerUsuarios();
        }
    }
}
