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

    InformingFcmSenderId,
    [Description("Google firebase server key")]
    InformingFcmServerKey,
    [Description("Sender email address")]
    InformingMailFrom,
    [Description("Mail display name")]
    InformingMailDisplayName,
    [Description("Mail server password")]
    InformingMailPassword,
    [Description("Mail server address")]
    InformingMailHost,
    [Description("Mail server port")]
    InformingMailPort,

    [Description("Customer referral code digits")]
    CustomerReferralCodeDigits,
    [Description("Customer referral code prefix")]
    CustomerReferralCodePrefix,
    [Description("Customer can define its referral less than this threshold")]
    CustomerAllowReferralThresholdInMinutes,

    [Description("Polygon mainnet url")]
    BlockChainPolygonMainNetRpcUrl,
    [Description("Polygon mainnet websocket")]
    BlockChainPolygonMainNetWebSocket,
    [Description("Polygon testnet url")]
    BlockChainPolygonTestNetRpcUrl,
    [Description("Polygon testnet websocket")]
    BlockChainPolygonTestNetWebSocket,
    [Description("Ethereum mainnet url")]
    BlockChainEthereumMainNetRpcUrl,
    [Description("Ethereum mainnet websocket")]
    BlockChainEthereumMainNetWebSocket,
    [Description("Ethereum testnet url")]
    BlockChainEthereumTestNetRpcUrl,
    [Description("Ethereum testnet websocket")]
    BlockChainEthereumTestNetWebSocket,
    [Description("CoinMarketCap API Key")]
    CoinMarketCapApiKey,

    [Description("Polygon mainnet url")]
    BlockChainLogProcessorPolygonMainNetRpcUrl,
    [Description("Polygon mainnet websocket")]
    BlockChainLogProcessorPolygonMainNetWebSocket,
    [Description("Polygon testnet url")]
    BlockChainLogProcessorPolygonTestNetRpcUrl,
    [Description("Polygon testnet websocket")]
    BlockChainLogProcessorPolygonTestNetWebSocket,
    [Description("Ethereum mainnet url")]
    BlockChainLogProcessorEthereumMainNetRpcUrl,
    [Description("Ethereum mainnet websocket")]
    BlockChainLogProcessorEthereumMainNetWebSocket,
    [Description("Ethereum testnet url")]
    BlockChainLogProcessorEthereumTestNetRpcUrl,
    [Description("Ethereum testnet websocket")]
    BlockChainLogProcessorEthereumTestNetWebSocket,
    [Description("Wait interval between inquiry log from rpc in seconds")]
    BlockChainLogProcessorWaitInterval,
    [Description("Number of blocks that inquiry per batch")]
    BlockChainLogProcessorBlocksPerBatch,
    [Description("Request retry weight")]
    BlockChainLogProcessorRequestRetryWeight,
    [Description("Minimum block confirmation that needs to process the block")]
    BlockChainLogProcessorMinimumBlockConfirmations,


    [Description("Polygon mainnet url")]
    BlockChainPaymentPolygonMainNetRpcUrl,
    [Description("Polygon mainnet websocket")]
    BlockChainPaymentPolygonMainNetWebSocket,
    [Description("Polygon testnet url")]
    BlockChainPaymentPolygonTestNetRpcUrl,
    [Description("Polygon testnet websocket")]
    BlockChainPaymentPolygonTestNetWebSocket,
    [Description("Ethereum mainnet url")]
    BlockChainPaymentEthereumMainNetRpcUrl,
    [Description("Ethereum mainnet websocket")]
    BlockChainPaymentEthereumMainNetWebSocket,
    [Description("Ethereum testnet url")]
    BlockChainPaymentEthereumTestNetRpcUrl,
    [Description("Ethereum testnet websocket")]
    BlockChainPaymentEthereumTestNetWebSocket,
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

    [Description("Polygon mainnet url")]
    BlockChainTransferPolygonMainNetRpcUrl,
    [Description("Polygon mainnet websocket")]
    BlockChainTransferPolygonMainNetWebSocket,
    [Description("Polygon testnet url")]
    BlockChainTransferPolygonTestNetRpcUrl,
    [Description("Polygon testnet websocket")]
    BlockChainTransferPolygonTestNetWebSocket,
    [Description("Ethereum mainnet url")]
    BlockChainTransferEthereumMainNetRpcUrl,
    [Description("Ethereum mainnet websocket")]
    BlockChainTransferEthereumMainNetWebSocket,
    [Description("Ethereum testnet url")]
    BlockChainTransferEthereumTestNetRpcUrl,
    [Description("Ethereum testnet websocket")]
    BlockChainTransferEthereumTestNetWebSocket,

    [Description("Block chain identity default generated siwe message lifetime in seconds")]
    BlockChainIdentityDefaultGeneratedSiweMessageLifeTime,
    [Description("Block chain identity secret to encrypt jwt token")]
    BlockChainIdentitySecret,
    BlockChainIdentityPolygonMainNetRpcUrl,
    BlockChainIdentityPolygonMainNetWebSocket,
    BlockChainIdentityPolygonTestNetRpcUrl,
    BlockChainIdentityPolygonTestNetWebSocket,


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