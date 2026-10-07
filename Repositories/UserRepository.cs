using sof1_IIPA26.Dtos;
using sof1_IIPA26.Utilities;
using Microsoft.Data.SqlClient;

namespace sof1_IIPA26.Repositories
{
    public class UserRepository
    {

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
    }
}
