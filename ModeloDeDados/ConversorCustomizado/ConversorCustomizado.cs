using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ModeloDeDados.Domain;

namespace ModeloDeDados.Converters;

public class ConversorCustomizado : ValueConverter<Status, char>
{
    public ConversorCustomizado() : base(
        p => (ConverterParaOhBancoDeDados(p)),
        value => ConverterParaAplicação(value),
        new ConverterMappingHints(1)) // o tamanho do campo no bd
    {

    }

    // ConvertToProvider
    private static char ConverterParaOhBancoDeDados(Status status)
    {
        return char.Parse(status.ToString()[0..1]);
    }

    // ConvertFromProvider
    private static Status ConverterParaAplicação(char value)
    {
        var status = Enum
            .GetValues<Status>()
            .FirstOrDefault(p => char.Parse(p.ToString()[0..1]) == value);

        return status;
    }
}