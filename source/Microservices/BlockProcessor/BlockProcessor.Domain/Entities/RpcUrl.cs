using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BlockProcessor.Domain.Entities;

public class RpcUrl : BaseEntity
{
    [Required]
    public Nethereum.Signer.Chain Chain { get; set; }

    private string _url = string.Empty;
    [Required]
    public Uri Uri
    {
        get => new(_url);
        set => _url = value.ToString();
    }

    [Required]
    public int BlockOfConfirmation { get; set; } = 3;

    /// <summary>
    /// Wait interval between each block processing step in log processor
    /// </summary>
    [Required]
    public int WaitInterval { get; set; } = 10;

    /// <summary>
    /// Maximum number of blocks for fetch from database to process in log processor in each step of processing
    /// </summary>
    [Required]
    public int MaxNumberOfBlocksPerProcess {get; set;} = 4;

    /// <summary>
    /// Maximum degree of parallelism in each log processing step
    /// </summary>
    [Required]
    public int MaxDegreeOfParallelism {get; set;} = 2;

    public bool Enabled { get; set; } = true;

    public string? ErrorMessage { get; set; }
}