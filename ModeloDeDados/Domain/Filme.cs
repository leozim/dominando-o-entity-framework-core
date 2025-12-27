namespace ModeloDeDados.Domain;

public class Filme
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Ator> Atores { get; } = new List<Ator>();
}