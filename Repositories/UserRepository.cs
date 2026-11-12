using sof1_IIPA26.Dtos;
using sof1_IIPA26.Utilities;
using sof1_IIPA26.Repositories.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace sof1_IIPA26.Repositories
{
    public class UserRepository
    {
        private readonly TestContext _testContext;

        public UserRepository(TestContext testContext)
        {
            _testContext = testContext;
        }

        public async Task<Boolean> CreateUser(UserDto userDto)
        {
            //Consulta SQL
            bool result = false;
            string SQL = "INSERT INTO TEST.dbo.[USER] (id_role,id_state,name,username,password) VALUES (" + userDto.IdRole + "," + userDto.IdState + ",'" + userDto.Name + "','" + userDto.Username + "','" + userDto.Password + "');";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                int comando = command.ExecuteNonQuery();
                if (comando != 0)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            Connection.Disconnect();

            return result;
        }

        public async Task<Boolean> Login(LoginDto loginDto)
        {
            //Consulta SQL
            bool result = false;
            string SQL = "SELECT * FROM TEST.dbo.[USER] WHERE username = '" + loginDto.NombreUsuario + "' AND password = '" + loginDto.Contrasena + "';";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            Connection.Disconnect();

            return result;
        }

        public async Task<List<UserDto>> ObtenerUsuarios()
        {
            List<UserDto> listResponseUserDto = new List<UserDto>();
            //Consulta SQL
            string SQL = "SELECT U.id_user,U.id_role,R.name AS role,U.id_state,S.name AS state,U.name,U.username,U.password " +
                "FROM TEST.dbo.[USER] U " +
                "JOIN TEST.dbo.[ROLE] R ON R.id_rol = U.id_role " +
                "INNER JOIN TEST.dbo.[STATE] S ON S.id_state = U.id_state;";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        listResponseUserDto.Add(new UserDto
                        {
                            IdUser = reader.GetInt32(0),
                            IdRole = reader.GetInt32(1),
                            Role = reader.GetString(2),
                            IdState = reader.GetInt32(3),
                            State = reader.GetString(4),
                            Name = reader.GetString(5),
                            Username = reader.GetString(6),
                            Password = reader.GetString(7)
                        });
                    }
                }
            }
            Connection.Disconnect();

            return listResponseUserDto;
        }

        /// <summary>
        /// Crea un nuevo usuario usando Entity Framework Core
        /// Alternativa moderna a CreateUser (SQL directo)
        /// </summary>
        public async Task<Boolean> CreateUserEF(UserDto userDto)
        {
            try
            {
                // Crear nueva entidad User a partir del DTO
                var newUser = new User
                {
                    IdRole = userDto.IdRole,
                    IdState = userDto.IdState,
                    Name = userDto.Name,
                    Username = userDto.Username,
                    Password = userDto.Password
                };

                _testContext.Users.Add(newUser);
                int recordsAffected = await _testContext.SaveChangesAsync();

                return recordsAffected > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al crear usuario con EF: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Valida login usando Entity Framework Core con LINQ
        /// Alternativa moderna a Login (SQL directo) - retorna UserDto con datos si existe
        /// </summary>
        public async Task<UserDto?> LoginEF(LoginDto loginDto)
        {
            try
            {
                var user = await _testContext.Users
                        .Include(u => u.IdRoleNavigation)
                        .Include(u => u.IdStateNavigation)
                        .FirstOrDefaultAsync(u =>
                            u.Username == loginDto.NombreUsuario &&
                            u.Password == loginDto.Contrasena);

                if (user == null) return null;

                return new UserDto
                {
                    IdUser = user.IdUser,
                    IdRole = user.IdRole,
                    Role = user.IdRoleNavigation != null ? user.IdRoleNavigation.Name : string.Empty,
                    IdState = user.IdState,
                    State = user.IdStateNavigation != null ? user.IdStateNavigation.Name : string.Empty,
                    Name = user.Name
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al validar login con EF: {ex.Message}");
                return null;
            }
        }

        ///// <summary>
        ///// Obtiene todos los usuarios con su información de rol y estado usando EF + LINQ
        ///// Alternativa moderna a ObtenerUsuarios (SQL directo)
        ///// </summary>
        //public async Task<List<UserDto>> ObtenerUsuariosEF()
        //{
        //    try
        //    {
        //        // LINQ Include para cargar relaciones (equivalente a JOIN en SQL)
        //        var usuarios = await _testContext.Users
        //            .Include(u => u.IdRoleNavigation)  // Carga la relación con Role
        //            .Include(u => u.IdStateNavigation)  // Carga la relación con State
        //            .Select(u => new UserDto
        //            {
        //                IdUser = u.IdUser,
        //                IdRole = u.IdRole ?? 0,
        //                Role = u.IdRoleNavigation != null ? u.IdRoleNavigation.Name : "Sin asignar",
        //                IdState = u.IdState,
        //                State = u.IdStateNavigation != null ? u.IdStateNavigation.Name : "Desconocido",
        //                Name = u.Name,
        //                Username = u.Username,
        //                Password = u.Password
        //            })
        //            .ToListAsync();

        //        return usuarios;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al obtener usuarios con EF: {ex.Message}");
        //        return new List<UserDto>();
        //    }
        //}

        ///// <summary>
        ///// Busca un usuario por ID usando EF + LINQ
        ///// </summary>
        //public async Task<UserDto> ObtenerUsuarioPorIdEF(int idUser)
        //{
        //    try
        //    {
        //        var usuario = await _testContext.Users
        //                .Include(u => u.IdRoleNavigation)
        //                .Include(u => u.IdStateNavigation)
        //                .Where(u => u.IdUser == idUser)
        //                .Select(u => new UserDto
        //                {
        //                    IdUser = u.IdUser,
        //                    IdRole = u.IdRole,
        //                    Role = u.IdRoleNavigation != null ? u.IdRoleNavigation.Name : "Sin asignar",
        //                    IdState = u.IdState,
        //                    State = u.IdStateNavigation != null ? u.IdStateNavigation.Name : "Desconocido",
        //                    Name = u.Name,
        //                    Username = u.Username,
        //                    Password = u.Password
        //                })
        //                .FirstOrDefaultAsync();

        //        return usuario ?? new UserDto();
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al obtener usuario por ID con EF: {ex.Message}");
        //        return new UserDto();
        //    }
        //}

        ///// <summary>
        ///// Actualiza información de un usuario usando EF + LINQ
        ///// </summary>
        //public async Task<Boolean> ActualizarUsuarioEF(int idUser, UserDto userDto)
        //{
        //    try
        //    {
        //        // Buscar el usuario existente
        //        var usuario = await _testContext.Users.FindAsync(idUser);

        //        if (usuario == null)
        //            return false;

        //        // Actualizar propiedades
        //        usuario.Name = userDto.Name ?? usuario.Name;
        //        usuario.Username = userDto.Username ?? usuario.Username;
        //        usuario.Password = userDto.Password ?? usuario.Password;

        //        // IdRole puede ser null
        //        if (userDto.IdRole.HasValue)
        //            usuario.IdRole = userDto.IdRole.Value;

        //        usuario.IdState = userDto.IdState;

        //        // EF Core rastrea cambios automáticamente
        //        int recordsAffected = await _testContext.SaveChangesAsync();
        //        return recordsAffected > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al actualizar usuario con EF: {ex.Message}");
        //        return false;
        //    }
        //}

        ///// <summary>
        ///// Elimina un usuario usando EF + LINQ
        ///// </summary>
        //public async Task<Boolean> EliminarUsuarioEF(int idUser)
        //{
        //    try
        //    {
        //        // Buscar el usuario
        //        var usuario = await _testContext.Users.FindAsync(idUser);

        //        if (usuario == null)
        //            return false;

        //        // Marcar para eliminación
        //        _testContext.Users.Remove(usuario);
        //        int recordsAffected = await _testContext.SaveChangesAsync();

        //        return recordsAffected > 0;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al eliminar usuario con EF: {ex.Message}");
        //        return false;
        //    }
        //}
    }
}
