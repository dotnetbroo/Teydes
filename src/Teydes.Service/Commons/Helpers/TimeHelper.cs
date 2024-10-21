using Teydes.Service.Commons.Constants;

namespace Teydes.Service.Commons.Helpers;

public class TimeHelper
{
    public static DateTime GetCurrentServerTime()
    {
        var date = DateTime.UtcNow;
        return date.AddHours(TimeConstants.UTC);
    }
}
