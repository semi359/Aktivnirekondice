namespace ClankyWeb.Data;

public static class Utils
{
    public static string ToDaysString(this DateTime date)
    {
        if (date == null)
            return String.Empty;

        int days = (int)(date - DateTime.Today).TotalDays;


        switch (days)
        {
            case -7: return $"před týdnem";
            case -2: return $"předevčírem";
            case -1: return $"včera";
            case 0: return $"dnes";
            case < -2: return $"před {-days} dny";
            default: return date.ToString("yyyy-MM-dd");
        }
    }


    public static bool IsNickNameUniqueException(Exception ex)
    => IsUniqueException(ex, "AspNetUsers", "NickName");

    public static bool IsUniqueException(Exception ex, string tableName, string columnName)
    {
        var inner = ex?.InnerException;
        if (inner?.Message == null)
            return false;

        var msg = inner.Message;

        var hasTable = msg.Contains($"dbo.{tableName}", StringComparison.OrdinalIgnoreCase);
        var hasIndex = msg.Contains($"{tableName}_{columnName}", StringComparison.OrdinalIgnoreCase);
        var hasUnique = msg.Contains("unique index", StringComparison.OrdinalIgnoreCase)
                     || msg.Contains("duplicate key", StringComparison.OrdinalIgnoreCase);

        return hasTable && hasIndex && hasUnique;
    }

}
