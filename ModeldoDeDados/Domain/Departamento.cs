using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ModeloDeDados.Domain;

public class Departamento
{
    public int Id { get; set; }
    public string Descricao { get; set; }
    public bool Ativo { get; set; }
    public bool Excluido { get; set; }
    
    public virtual List<Funcionario> Funcionarios { get; set; }
}
