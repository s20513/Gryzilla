namespace Gryzilla_App.Helpers;

public class DateTimeConverter
{
    public static string GetDateTimeToStringWithFormat(DateTime? dateTime)
    {
        if (dateTime is null)
        {
            return "";
        }

        var notNullDateTime = (DateTime)dateTime;
        
        return notNullDateTime.ToString("hh:mm yyyy-M-dd");
    }
}