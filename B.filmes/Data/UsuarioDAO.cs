using B.filmes.Models;
//using BibliotecaFilmes.Models;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace BibliotecaFilmes.Data
{
    public class UsuarioDAO
    {
        private readonly string connectionString;

        public UsuarioDAO(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public Usuario Login(string email, string senha)
        {
            using var conn = new MySqlConnection(connectionString);
            conn.Open();
            var cmd = new MySqlCommand("SELECT * FROM Usuarios WHERE Email = @Email AND Senha = @Senha", conn);
            cmd.Parameters.AddWithValue("@Email", email);
            cmd.Parameters.AddWithValue("@Senha", senha);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                return new Usuario
                {
                    Id = reader.GetInt32("Id"),
                    Nome = reader.GetString("Nome"),
                    Email = reader.GetString("Email"),
                    Senha = reader.GetString("Senha"),
                    Tipo = reader.GetString("Tipo")
                };
            }

            return null;
        }
    }
}

