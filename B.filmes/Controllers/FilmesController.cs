using Microsoft.AspNetCore.Mvc;
using B.filmes.Models;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace B.filmes.Controllers
{
    public class FilmeController : Controller
    {
        private readonly string _connectionString;

        public FilmeController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        // Listar todos os filmes
        public IActionResult Index()
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

        // GET: Criar filme
        [HttpGet]
        public IActionResult Criar()
        {
            return View();
        }

        // POST: Criar filme
        [HttpPost]
        public IActionResult Criar(Filme filme)
        {
            if (!ModelState.IsValid)
                return View(filme);

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                @"INSERT INTO filmes (Titulo, Ano, Genero, Diretor, Duracao, Sinopse, LinkTrailer, LinkFilme, Plataformas, ImagemCapa) 
                  VALUES (@Titulo, @Ano, @Genero, @Diretor, @Duracao, @Sinopse, @LinkTrailer, @LinkFilme, @Plataformas, @ImagemCapa)", conn);

            cmd.Parameters.AddWithValue("@Titulo", filme.Titulo);
            cmd.Parameters.AddWithValue("@Ano", (object)filme.Ano ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Genero", filme.Genero ?? "");
            cmd.Parameters.AddWithValue("@Diretor", filme.Diretor ?? "");
            cmd.Parameters.AddWithValue("@Duracao", filme.Duracao ?? "");
            cmd.Parameters.AddWithValue("@Sinopse", filme.Sinopse ?? "");
            cmd.Parameters.AddWithValue("@LinkTrailer", filme.LinkTrailer ?? "");
            cmd.Parameters.AddWithValue("@LinkFilme", filme.LinkFilme ?? "");
            cmd.Parameters.AddWithValue("@Plataformas", filme.Plataformas ?? "");
            cmd.Parameters.AddWithValue("@ImagemCapa", filme.ImagemCapa ?? "");

            cmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        // GET: Editar filme
        [HttpGet]
        public IActionResult Editar(int id)
        {
            Filme filme = null;

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM filmes WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                filme = new Filme
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
                };
            }

            if (filme == null)
                return NotFound();

            return View(filme);
        }

        // POST: Editar filme
        [HttpPost]
        public IActionResult Editar(Filme filme)
        {
            if (!ModelState.IsValid)
                return View(filme);

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand(
                @"UPDATE filmes SET 
                    Titulo = @Titulo, Ano = @Ano, Genero = @Genero, Diretor = @Diretor, Duracao = @Duracao, 
                    Sinopse = @Sinopse, LinkTrailer = @LinkTrailer, LinkFilme = @LinkFilme, Plataformas = @Plataformas, ImagemCapa = @ImagemCapa
                  WHERE Id = @Id", conn);

            cmd.Parameters.AddWithValue("@Titulo", filme.Titulo);
            cmd.Parameters.AddWithValue("@Ano", (object)filme.Ano ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Genero", filme.Genero ?? "");
            cmd.Parameters.AddWithValue("@Diretor", filme.Diretor ?? "");
            cmd.Parameters.AddWithValue("@Duracao", filme.Duracao ?? "");
            cmd.Parameters.AddWithValue("@Sinopse", filme.Sinopse ?? "");
            cmd.Parameters.AddWithValue("@LinkTrailer", filme.LinkTrailer ?? "");
            cmd.Parameters.AddWithValue("@LinkFilme", filme.LinkFilme ?? "");
            cmd.Parameters.AddWithValue("@Plataformas", filme.Plataformas ?? "");
            cmd.Parameters.AddWithValue("@ImagemCapa", filme.ImagemCapa ?? "");
            cmd.Parameters.AddWithValue("@Id", filme.Id);

            cmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        // GET: Deletar filme (confirmação)
        [HttpGet]
        public IActionResult Deletar(int id)
        {
            Filme filme = null;

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM filmes WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                filme = new Filme
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Titulo = reader["Titulo"].ToString()
                };
            }

            if (filme == null)
                return NotFound();

            return View(filme);
        }

        // POST: Deletar filme (ação)
        [HttpPost, ActionName("Deletar")]
        public IActionResult DeletarConfirmado(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("DELETE FROM filmes WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();

            return RedirectToAction(nameof(Index));
        }

        // GET: Detalhes do filme
        [HttpGet]
        public IActionResult Detalhes(int id)
        {
            Filme filme = null;

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var cmd = new MySqlCommand("SELECT * FROM filmes WHERE Id = @Id", conn);
            cmd.Parameters.AddWithValue("@Id", id);

            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                filme = new Filme
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
                };
            }

            if (filme == null)
                return NotFound();

            return View(filme);
        }
    }
}




