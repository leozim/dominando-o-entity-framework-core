namespace ModeloDeDados.Domain;

public class Ator
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Filme> Filmes { get; } = new List<Filme>();
}