using B.filmes.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace B.filmes.Data
{
    public class FilmeDAO
    {
        private readonly string _connectionString;

        public FilmeDAO(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Listar todos os filmes (opcional filtro)
        public List<Filme> Listar(string pesquisa = null)
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

            using var cmd = new MySqlCommand(sql, conn);

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

            return filmes;
        }

        // Obter filme por id
        public Filme ObterPorId(int id)
        {
            Filme filme = null;

            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "SELECT * FROM filmes WHERE Id = @id LIMIT 1";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);

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

            return filme;
        }

        // Inserir novo filme
        public void Inserir(Filme filme)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = @"
                INSERT INTO filmes (Titulo, Ano, Genero, Diretor, Duracao, Sinopse, LinkTrailer, LinkFilme, Plataformas, ImagemCapa)
                VALUES (@Titulo, @Ano, @Genero, @Diretor, @Duracao, @Sinopse, @LinkTrailer, @LinkFilme, @Plataformas, @ImagemCapa)";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Titulo", filme.Titulo);
            cmd.Parameters.AddWithValue("@Ano", (object)filme.Ano ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Genero", filme.Genero);
            cmd.Parameters.AddWithValue("@Diretor", filme.Diretor);
            cmd.Parameters.AddWithValue("@Duracao", filme.Duracao);
            cmd.Parameters.AddWithValue("@Sinopse", filme.Sinopse);
            cmd.Parameters.AddWithValue("@LinkTrailer", filme.LinkTrailer);
            cmd.Parameters.AddWithValue("@LinkFilme", filme.LinkFilme);
            cmd.Parameters.AddWithValue("@Plataformas", filme.Plataformas);
            cmd.Parameters.AddWithValue("@ImagemCapa", filme.ImagemCapa);

            cmd.ExecuteNonQuery();
        }

        // Atualizar filme existente
        public void Atualizar(Filme filme)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = @"
                UPDATE filmes SET
                    Titulo = @Titulo,
                    Ano = @Ano,
                    Genero = @Genero,
                    Diretor = @Diretor,
                    Duracao = @Duracao,
                    Sinopse = @Sinopse,
                    LinkTrailer = @LinkTrailer,
                    LinkFilme = @LinkFilme,
                    Plataformas = @Plataformas,
                    ImagemCapa = @ImagemCapa
                WHERE Id = @Id";

            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Titulo", filme.Titulo);
            cmd.Parameters.AddWithValue("@Ano", (object)filme.Ano ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@Genero", filme.Genero);
            cmd.Parameters.AddWithValue("@Diretor", filme.Diretor);
            cmd.Parameters.AddWithValue("@Duracao", filme.Duracao);
            cmd.Parameters.AddWithValue("@Sinopse", filme.Sinopse);
            cmd.Parameters.AddWithValue("@LinkTrailer", filme.LinkTrailer);
            cmd.Parameters.AddWithValue("@LinkFilme", filme.LinkFilme);
            cmd.Parameters.AddWithValue("@Plataformas", filme.Plataformas);
            cmd.Parameters.AddWithValue("@ImagemCapa", filme.ImagemCapa);
            cmd.Parameters.AddWithValue("@Id", filme.Id);

            cmd.ExecuteNonQuery();
        }

        // Deletar filme por id
        public void Deletar(int id)
        {
            using var conn = new MySqlConnection(_connectionString);
            conn.Open();

            var sql = "DELETE FROM filmes WHERE Id = @Id";
            using var cmd = new MySqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
