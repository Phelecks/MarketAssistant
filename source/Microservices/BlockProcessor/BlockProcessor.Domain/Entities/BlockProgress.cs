using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class BlockProgress : BaseAuditEntity
{
    [Required]
    public long BlockNumber { get; set; }

    private int _chain;
    [Required]
    public Nethereum.Signer.Chain Chain
    {
        get => (Nethereum.Signer.Chain)_chain;
        set => _chain = (int)value;
    }

    public BlockProgressStatus Status { get; set; }

    public enum BlockProgressStatus
    {
        Processed,
        Processing,
        Failed
    }
}