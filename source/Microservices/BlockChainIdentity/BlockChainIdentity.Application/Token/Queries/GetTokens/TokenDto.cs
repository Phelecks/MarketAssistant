using BaseApplication.Mappings;
using Microsoft.EntityFrameworkCore;

namespace BlockChainIdentity.Application.Token.Queries.GetTokens;

public class TokenDto : IMapFrom<Domain.Entities.Token>
{
    public long id { get; set; }
    /// <summary>
    /// ISO 8601 datetime string of the current time. 
    /// </summary>
    public DateTime issuedAt { get; set; }

    /// <summary>
    /// ISO 8601 datetime string that, if present, indicates when the signed authentication message is no longer valid. 
    /// </summary>
    public DateTime expireAt { get; set; }

    /// <summary>
    /// ISO 8601 datetime string that, if present, indicates when the signed authentication message will become valid. 
    /// </summary>
    public DateTime notBefore { get; set; }

    /// <summary>
    /// Human-readable ASCII assertion that the user will sign, and it must not contain `\n`. 
    /// </summary>
    public string statement { get; set; }

    /// <summary>
    /// RFC 3986 URI referring to the resource that is the subject of the signing
    /// (as in the __subject__ of a claim).
    /// </summary>
    private string url { get; set; }
    //[NotMapped]
    [BackingField(nameof(url))]
    public Uri uri { get { return new Uri(url); } set { url = value.AbsoluteUri; } }

    /// <summary>
    /// Current version of the message. 
    /// </summary>
    public string version { get; set; }

    /// <summary>
    /// Randomized token used to prevent replay attacks, at least 8 alphanumeric characters. 
    /// </summary>
    public string nonce { get; set; }

    /// <summary>
    /// System-specific identifier that may be used to uniquely refer to the sign-in request
    /// </summary>

    public string requestId { get; set; }

    /// <summary>
    /// List of information or references to information the user wishes to have resolved as part of authentication by the relying party. They are expressed as RFC 3986 URIs separated by `\n- `
    /// Comma separated
    /// </summary>
    public string resources { get; set; }

    /// <summary>
    /// Control validity of token manualy
    /// </summary>
    public bool enabled { get; set; }
}
