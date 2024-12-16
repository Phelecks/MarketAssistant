using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateNftReferralRewardDocumentMessage : ICreateNftReferralRewardDocumentMessage
{
    public CreateNftReferralRewardDocumentMessage(Guid correlationId, string transactionHash, DateTime dateTime, string erc721From, string erc721To, decimal referralRewardValue, float referralRewardPercent, string referralUserId, ICreateNftReferralRewardDocumentMessage.SmartContractDto smartContract, ICreateNftReferralRewardDocumentMessage.NftDto token, ICreateNftReferralRewardDocumentMessage.RelatedTradeDto relatedTrade)
    {
        CorrelationId = correlationId;
        TransactionHash = transactionHash;
        DateTime = dateTime;
        Erc721From = erc721From;
        Erc721To = erc721To;
        ReferralRewardValue = referralRewardValue;
        ReferralRewardPercent = referralRewardPercent;
        SmartContract = smartContract;
        Token = token;
        RelatedTrade = relatedTrade;
        ReferralUserId = referralUserId;
    }

    public Guid CorrelationId { get; }

    public string TransactionHash { get; }

    public DateTime DateTime { get; }

    public string Erc721From { get; }

    public string Erc721To { get; }

    public decimal ReferralRewardValue { get; }

    public float ReferralRewardPercent { get; }
    public string ReferralUserId { get; set; }

    public ICreateNftReferralRewardDocumentMessage.SmartContractDto SmartContract { get; }

    public ICreateNftReferralRewardDocumentMessage.NftDto Token { get; }

    public ICreateNftReferralRewardDocumentMessage.RelatedTradeDto RelatedTrade { get; }
}