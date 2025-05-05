using System.ComponentModel.DataAnnotations;
using BaseDomain.Entities;

namespace BlockProcessor.Domain.Entities;

public class RpcUrl : BaseRpcUrl
{
    [Required]
    public override int BlockOfConfirmation { get; set; } = 3;

    /// <summary>
    /// Wait interval between each block processing step in log processor
    /// </summary>
    [Required]
    public override int WaitInterval { get; set; } = 10;

    /// <summary>
    /// Maximum number of blocks for fetch from database to process in log processor in each step of processing
    /// </summary>
    [Required]
    public override int MaxNumberOfBlocksPerProcess {get; set;} = 4;

    /// <summary>
    /// Maximum degree of parallelism in each log processing step
    /// </summary>
    [Required]
    public override int MaxDegreeOfParallelism {get; set;} = 2;
}