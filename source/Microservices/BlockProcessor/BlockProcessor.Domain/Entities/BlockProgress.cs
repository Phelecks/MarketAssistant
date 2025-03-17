using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class BlockProgress : BaseAuditEntity
{
    [Required]
    public long BlockNumber { get; set; }

    [Required]
    public Nethereum.Signer.Chain Chain { get; set; }

    public BlockProgressStatus Status { get; set; }

    public enum BlockProgressStatus
    {
        Processed,
        Processing,
        Failed
    }
}