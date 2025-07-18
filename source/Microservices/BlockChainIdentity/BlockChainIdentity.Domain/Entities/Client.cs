using EntityFrameworkCore.EncryptColumn.Attribute;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using BaseDomain.Common;

namespace BlockChainIdentity.Domain.Entities;

public class Client : BaseAuditEntity
{
    [Required]
    public required string ClientId { get; set; }

    [Required]
    [EncryptColumn]
    public required string ClientSecret { get; set; }

    private string Url { get; set; }
    [BackingField(nameof(Url))]
    public Uri Uri { get { return new Uri(Url); } set { Url = value.AbsoluteUri; } }

    [Required]
    public int TokenLifeTimeInSeconds { get; set; }

    public bool Enabled { get; set; }

    [Required]
    public required string Statement { get; set; }

    [Required]
    public required string Version { get; set; }

    public virtual ICollection<ClientResource> ClientResources { get; set; }
}