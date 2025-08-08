using System.Diagnostics;
using B.filmes.Models;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace B.filmes.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly string _connectionString;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Menu do Administrador - lista todos os filmes
        public IActionResult MenuAdmin()
        {
            var filmes = new List<Filme>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM filmes ORDER BY DataCadastro DESC", conn);

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                filmes.Add(new Filme
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Titulo = reader["Titulo"].ToString(),
                    Ano = reader["Ano"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Ano"]),
                    Genero = reader["Genero"].ToString(),
                    Diretor = reader["Diretor"].ToString(),
                    Duracao = reader["Duracao"].ToString(),
                    Sinopse = reader["Sinopse"].ToString(),
                    LinkTrailer = reader["LinkTrailer"].ToString(),
                    LinkFilme = reader["LinkFilme"].ToString(),
                    Plataformas = reader["Plataformas"].ToString(),
                    ImagemCapa = reader["ImagemCapa"].ToString(),
                    DataCadastro = Convert.ToDateTime(reader["DataCadastro"])
                });
            }

            return View(filmes);
        }

        // Menu do Usuário Comum - lista filmes, permite filtro por título ou gênero
        public IActionResult MenuUsuario(string pesquisa = null)
        {
            var filmes = new List<Filme>();

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM filmes";
            if (!string.IsNullOrEmpty(pesquisa))
            {
                sql += " WHERE Titulo LIKE @pesquisa OR Genero LIKE @pesquisa";
            }
            sql += " ORDER BY DataCadastro DESC";

            var cmd = new MySqlCommand(sql, conn);

            if (!string.IsNullOrEmpty(pesquisa))
            {
                cmd.Parameters.AddWithValue("@pesquisa", $"%{pesquisa}%");
            }

            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                filmes.Add(new Filme
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Titulo = reader["Titulo"].ToString(),
                    Ano = reader["Ano"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["Ano"]),
                    Genero = reader["Genero"].ToString(),
                    Diretor = reader["Diretor"].ToString(),
                    Duracao = reader["Duracao"].ToString(),
                    Sinopse = reader["Sinopse"].ToString(),
                    LinkTrailer = reader["LinkTrailer"].ToString(),
                    LinkFilme = reader["LinkFilme"].ToString(),
                    Plataformas = reader["Plataformas"].ToString(),
                    ImagemCapa = reader["ImagemCapa"].ToString(),
                    DataCadastro = Convert.ToDateTime(reader["DataCadastro"])
                });
            }

            return View(filmes);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}


