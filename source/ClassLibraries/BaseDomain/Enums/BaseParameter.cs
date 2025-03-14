using System.ComponentModel;

namespace BaseDomain.Enums;

/// <summary>
/// Base parameter categories
/// </summary>
public enum BaseParameterCategory
{
    /// <summary>
    /// System configuration
    /// </summary>
    [Description("System configuiration")]
    SystemConfiguration,

    /// <summary>
    /// Identity configurations
    /// </summary>
    [Description("Identity configuiration")]
    IdentityConfiguration,

    /// <summary>
    /// Informing configuration
    /// </summary>
    [Description("Informing configuiration")]
    InformingConfiguration,

    /// <summary>
    /// Financial configuration
    /// </summary>
    [Description("Financial configuration")]
    FinancialConfiguration,

    /// <summary>
    /// Messaging configuration
    /// </summary>
    [Description("Messaging configuration")]
    MessagingConfiguration,

    CustomerConfiguration,
    CustomerClubConfiguration,
    CatalogConfiguration,
    BasketConfiguration,
    OrderConfiguration,
    BlockChainConfiguration,
    BlockChainLogProcessorConfiguration,
    BlockChainPaymentConfiguration,
    BlockChainTransferConfiguration,
    BlockChainIdentityConfiguration,
    GameConfiguration
}
/// <summary>
/// Base parameter fields
/// </summary>
public enum BaseParameterField
{
    /// <summary>
    /// Default language
    /// </summary>
    [Description("Default language")]
    DefaultLanguage,
    [Description("Application name")]
    ApplicationName,
    [Description("Application url")]
    ApplicationUrl,

    [Description("Customer referral code digits")]
    CustomerReferralCodeDigits,
    [Description("Customer referral code prefix")]
    CustomerReferralCodePrefix,
    [Description("Customer can define its referral less than this threshold")]
    CustomerAllowReferralThresholdInMinutes,

    [Description("CoinMarketCap API Key")]
    CoinMarketCapApiKey,

    [Description("Wait interval between inquiry log from rpc in seconds")]
    BlockChainLogProcessorWaitInterval,
    [Description("Number of blocks that inquiry per batch")]
    BlockChainLogProcessorBlocksPerBatch,
    [Description("Request retry weight")]
    BlockChainLogProcessorRequestRetryWeight,
    [Description("Minimum block confirmation that needs to process the block")]
    BlockChainLogProcessorMinimumBlockConfirmations,

    [Description("Wait interval between inquiry log from rpc in seconds")]
    BlockChainPaymentWaitInterval,
    [Description("Number of blocks that inquiry per batch")]
    BlockChainPaymentBlocksPerBatch,
    [Description("Request retry weight")]
    BlockChainPaymentRequestRetryWeight,
    [Description("Minimum block confirmation that needs to process the block to initiate the payment")]
    BlockChainPaymentMinimumBlockConfirmationsToInitiatePayment,
    [Description("Minimum block confirmation that needs to process the block to confirm the payment")]
    BlockChainPaymentMinimumBlockConfirmationsToConfirmPayment,
    [Description("Block chain identity default generated siwe message lifetime in seconds")]
    BlockChainIdentityDefaultGeneratedSiweMessageLifeTime,


    GameMatchIntervalMinutes,
    GameCompanyFeePercent,
    GameWithdrawWalletAddress,
    GameReferralPecent,
    /// <summary>
    /// TimeOnly
    /// </summary>
    GameStartsAt,
    GameOptionThumbnailBaseUrl,
    GameMinimumBetValue,
    GameCloseMatchIntervalMinutes,
    GameThumbnailBaseUrl,
    GameOneTimeStreak,

    CustomerClubDefaultGameReferralPercent,

    GameDiscordChannelId,
    InformingDiscordBotToken,
    GameOverviewNotifyIntervalInMinutes,
    BlockChainTransferOrphanTransferThresholdInMinutes
}