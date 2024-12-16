using System.Numerics;

namespace BaseApplication.Interfaces;

public interface IDateTimeService
{
    DateTime UtcNow { get; }
    DateTime Now { get; }

    DateTime ConvertFromUnixTimestamp(BigInteger timestamp);
}
