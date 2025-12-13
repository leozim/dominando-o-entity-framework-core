using System;

namespace DominandoEFCore.Domain;

public class Departamento
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public bool Ativo { get; set; }

    public List<Funcionario> Funcionarios { get; set; }
}
