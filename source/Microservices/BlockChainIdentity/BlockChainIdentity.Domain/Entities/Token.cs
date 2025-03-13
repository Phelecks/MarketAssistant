using BaseDomain.Common;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BlockChainIdentity.Domain.Entities;

public class Token : BaseEntity
{
    /// <summary>
    /// ISO 8601 datetime string of the current time. 
    /// </summary>
    public DateTime IssuedAt { get; set; }

    /// <summary>
    /// ISO 8601 datetime string that, if present, indicates when the signed authentication message is no longer valid. 
    /// </summary>
    public DateTime ExpireAt { get; set; }

    /// <summary>
    /// ISO 8601 datetime string that, if present, indicates when the signed authentication message will become valid. 
    /// </summary>
    public DateTime NotBefore { get; set; }

    /// <summary>
    /// Human-readable ASCII assertion that the user will sign, and it must not contain `\n`. 
    /// </summary>
    public required string statement { get; set; }

    /// <summary>
    /// RFC 3986 URI referring to the resource that is the subject of the signing
    /// (as in the __subject__ of a claim).
    /// </summary>
    private string Url { get; set; }
    //[NotMapped]
    [BackingField(nameof(Url))]
    public Uri Uri { get { return new Uri(Url); } set { Url = value.AbsoluteUri; } }

    /// <summary>
    /// Current version of the message. 
    /// </summary>
    public required string version { get; set; }

    /// <summary>
    /// Randomized token used to prevent replay attacks, at least 8 alphanumeric characters. 
    /// </summary>
    public required string Nonce { get; set; }

    /// <summary>
    /// System-specific identifier that may be used to uniquely refer to the sign-in request
    /// </summary>

    public required string RequestId { get; set; }

    /// <summary>
    /// List of information or references to information the user wishes to have resolved as part of authentication by the relying party. They are expressed as RFC 3986 URIs separated by `\n- `
    /// Comma separated
    /// </summary>
    public required string Resources { get; set; }

    /// <summary>
    /// Control validity of token manualy
    /// </summary>
    public bool Enabled { get; set; }



    [ForeignKey("Wallet")]
    public string WalletAddress { get; set; }
    public virtual Wallet Wallet { get; set; }
}