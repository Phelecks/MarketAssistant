using BaseApplication.Interfaces;
using System.Numerics;

namespace BaseInfrastructure.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime UtcNow => DateTime.UtcNow;
    public DateTime Now => DateTime.Now;
    public DateTime ConvertFromUnixTimestamp(BigInteger timestamp)
    {
        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0);
        return origin.AddSeconds((double)timestamp);
    }
}
