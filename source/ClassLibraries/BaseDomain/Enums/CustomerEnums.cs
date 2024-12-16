using System.ComponentModel;

namespace BaseDomain.Enums;

public class CustomerEnums
{
    public enum CustomerLevel
    {
        [Description("Level 0")]
        Zero,
        [Description("Level 1")]
        One,
        [Description("Level 2")]
        Two
    }
}