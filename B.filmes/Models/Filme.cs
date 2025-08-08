namespace B.filmes.Models
{
    public class Filme
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public int? Ano { get; set; }
        public string Genero { get; set; }
        public string Diretor { get; set; }
        public string Duracao { get; set; }
        public string Sinopse { get; set; }
        public string LinkTrailer { get; set; }
        public string LinkFilme { get; set; }
        public string Plataformas { get; set; }
        public string ImagemCapa { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}


