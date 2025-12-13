namespace DominandoEFCore.Domain;

public class Funcionario
{
    public Guid Id { get; set; }
    public string Nome { get; set; }
    public string Cpf { get; set; }

    public Guid DepartamentoId { get; set; }
    public Departamento Departamento { get; set; }
}