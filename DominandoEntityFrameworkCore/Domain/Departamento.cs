using System;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace DominandoEFCore.Domain;

public class Departamento
{
    public Guid Id { get; set; }
    public string Descricao { get; set; }
    public bool Ativo { get; set; }

    private Action<object, string> _lazyLoader { get; }
    
    public Departamento()
    {
        
    }

    private Departamento(Action<object, string> LazyLoader)
    {
        _lazyLoader = LazyLoader;
    }

    private List<Funcionario> _funcionarios;
    public List<Funcionario> Funcionarios
    {
        get
        {
            _lazyLoader?.Invoke(this, nameof(Funcionarios));
            
            return _funcionarios;
        }
        set => _funcionarios = value;
    }
    
    /*
     public Departamento()
    {
        
    }

    private Departamento(ILazyLoader LazyLoader)
    {
        _lazyLoader = LazyLoader;
    }

    private List<Funcionario> _funcionarios;
    public List<Funcionario> Funcionarios
    {
        get => _lazyLoader.Load(this, ref _funcionarios); 
        set => _funcionarios = value;
    }
    */
    // public virtual List<Funcionario> Funcionarios { get; set; }
}
