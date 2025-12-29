namespace ModeloDeDados.Domain;

public class Filme
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Descricao { get; set; }

    public ICollection<Ator> Atores { get; } = new List<Ator>();
}