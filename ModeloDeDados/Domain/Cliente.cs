namespace ModeloDeDados.Domain;

public class Cliente
{
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Telefone { get; set; }
    
    public Endereco Endereco = new Endereco();
}

public class Endereco
{
    public string Logadouro { get; set; }
    public string Bairro { get; set; }
    public string Cidade { get; set; }
    public string Estado { get; set; }
}
