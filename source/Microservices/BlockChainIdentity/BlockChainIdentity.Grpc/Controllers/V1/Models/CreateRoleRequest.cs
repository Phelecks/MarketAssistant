using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Grpc.Controllers.V1.Models;

public class CreateRoleRequest
{
    [Required]
    public required string Title { get; set; }
}