namespace BaseDomain.Common;

public class BaseAuditEntityWithNoPrimaryKey : BaseEntityWithNoPrimaryKey
{
    public DateTime created { get; set; }

    public string? createdBy { get; set; }

    public DateTime lastModified { get; set; }

    public string? lastModifiedBy { get; set; }
}