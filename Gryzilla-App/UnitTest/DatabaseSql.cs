namespace UnitTest;

public class DatabaseSql
{
    public static string GetTruncateSql()
    {
        var script = File.ReadAllText(@"../../../Gryzilla_Truncate.sql");

        return script;
    }
}