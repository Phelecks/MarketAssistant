using MassTransitManager.Messages.Interfaces;

namespace MassTransitManager.Messages;

public class CreateNftReferralRewardDocumentMessage(Guid correlationId, string transactionHash, DateTime dateTime, string erc721From, string erc721To, decimal referralRewardValue, float referralRewardPercent, string referralUserId, ICreateNftReferralRewardDocumentMessage.SmartContractDto smartContract, ICreateNftReferralRewardDocumentMessage.NftDto token, ICreateNftReferralRewardDocumentMessage.RelatedTradeDto relatedTrade) : ICreateNftReferralRewardDocumentMessage
{
    public Guid CorrelationId { get; } = correlationId;

    public string TransactionHash { get; } = transactionHash;

    public DateTime DateTime { get; } = dateTime;

    public string Erc721From { get; } = erc721From;

    public string Erc721To { get; } = erc721To;

    public decimal ReferralRewardValue { get; } = referralRewardValue;

    public float ReferralRewardPercent { get; } = referralRewardPercent;
    public string ReferralUserId { get; set; } = referralUserId;

    public ICreateNftReferralRewardDocumentMessage.SmartContractDto SmartContract { get; } = smartContract;

    public ICreateNftReferralRewardDocumentMessage.NftDto Token { get; } = token;

    public ICreateNftReferralRewardDocumentMessage.RelatedTradeDto RelatedTrade { get; } = relatedTrade;
}