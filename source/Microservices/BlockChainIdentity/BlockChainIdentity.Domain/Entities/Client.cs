using BaseDomain.Common;
using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockChainIdentity.Domain.Entities;

public class Client : BaseAuditEntity
{
    [Required]
    public string clientId { get; set; }

    [Required]
    [EncryptColumn]
    public string clientSecret { get; set; }

    private string url { get; set; }
    //[NotMapped]
    [BackingField(nameof(url))]
    public Uri uri { get { return new Uri(url); } set { url = value.AbsoluteUri; } }

    [Required]
    public int tokenLifeTimeInSeconds { get; set; }

    public bool enabled { get; set; }

    [Required]
    public string statement { get; set; }

    [Required]
    public string version { get; set; }

    public virtual ICollection<ClientResource> clientResources { get; set; }
}