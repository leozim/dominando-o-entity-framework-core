namespace ModeloDeDados.Domain;

public class Estado
{
    public int Id { get; set; }
    public string Nome { get; set; }
    // Ao não colocar o GovernadorId implicitamente dizemos que Estado é a classe não dependente(dominante) da relação
    // pois em teoria um Governador depende da existência de um Estado, já o Estado não depende do Governador
    public Governador Governador { get; set; }
}

public class Governador
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string Partido { get; set; }

    public int EstadoId { get; set; }
    public Estado Estado { get; set; }
}