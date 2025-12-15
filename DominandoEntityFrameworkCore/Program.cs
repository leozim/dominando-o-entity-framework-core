using DominandoEFCore.Data;

internal class Program
{
    private static void Main(string[] args)
    {
        EnsureCreatedAndDelete();
    }

    private static void EnsureCreatedAndDelete()
    {
        using var db = new ApplicationContext();
        db.Database.EnsureDeleted();
        db.Database.EnsureCreated();
    }
}