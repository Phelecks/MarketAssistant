using System.ComponentModel.DataAnnotations;

namespace BlockChainIdentity.Grpc.Controllers.V1.Models;

public class CreateRoleRequest
{
    [Required]
    public string Title { get; set; }
}