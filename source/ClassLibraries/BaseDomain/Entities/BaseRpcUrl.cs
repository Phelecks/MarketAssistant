using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BaseDomain.Entities;

public class BaseRpcUrl : BaseEntity
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
    public virtual int BlockOfConfirmation { get; set; }

    /// <summary>
    /// Wait interval between each block processing step in log processor
    /// </summary>
    [Required]
    public virtual int WaitInterval { get; set; }

    /// <summary>
    /// Maximum number of blocks for fetch from database to process in log processor in each step of processing
    /// </summary>
    [Required]
    public virtual int MaxNumberOfBlocksPerProcess {get; set;}

    /// <summary>
    /// Maximum degree of parallelism in each log processing step
    /// </summary>
    [Required]
    public virtual int MaxDegreeOfParallelism {get; set;}

    public bool Enabled { get; set; } = true;

    public string? ErrorMessage { get; set; }
}