namespace BaseDomain.Common;

public interface IContractTrigger
{
    List<Dictionary<string, object>> Properties { get; }
}