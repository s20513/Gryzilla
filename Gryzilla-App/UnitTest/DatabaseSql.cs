namespace UnitTest;

public static class DatabaseSql
{
    public static string GetTruncateSql()
    {
        var script = File.ReadAllText(@"../../../Gryzilla_Truncate.sql");

        return script;
    }
}