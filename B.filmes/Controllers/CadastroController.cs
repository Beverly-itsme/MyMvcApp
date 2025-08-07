using Microsoft.AspNetCore.Mvc;
using B.filmes.Models;
using MySql.Data.MySqlClient;

namespace B.filmes.Controllers
{
    public class CadastroController : Controller
    {
        private readonly string _connectionString;

        public CadastroController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Usuario usuario)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            // Verificar se o e-mail já existe
            var checkCmd = new MySqlCommand("SELECT COUNT(*) FROM usuarios WHERE email = @Email", conn);
            checkCmd.Parameters.AddWithValue("@Email", usuario.Email);
            var count = Convert.ToInt32(checkCmd.ExecuteScalar());

            if (count > 0)
            {
                ViewBag.Erro = "Email já está em uso!";
                return View();
            }

            // Inserir novo usuário
            var cmd = new MySqlCommand("INSERT INTO usuarios (nome, email, senha) VALUES (@Nome, @Email, @Senha)", conn);
            cmd.Parameters.AddWithValue("@Nome", usuario.Nome);
            cmd.Parameters.AddWithValue("@Email", usuario.Email);
            cmd.Parameters.AddWithValue("@Senha", usuario.Senha); // opcional: criptografar

            cmd.ExecuteNonQuery();

            return RedirectToAction("Index", "Login");
        }
    }
}

