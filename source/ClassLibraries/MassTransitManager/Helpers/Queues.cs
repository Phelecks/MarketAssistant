namespace MassTransitManager.Helpers;

public static class Queues
{
    #region General
    public const string UpdateInformingContactMessageQueueName = "update-informing-contact-message-queue";
    public const string SendGeneralVerificationCodeMessageQueueName = "send-general-verification-code-message-queue";
    public const string CreateContractTriggerQueueName = "create-contract-trigger-event-queue";
    public const string CreateResourceMessageQueueName = "create-resource-message-queue";
    public const string SubmitSystemErrorMessageQueueName = "submit-system-error-message-queue";
    #endregion

    #region create-user-flow
    public const string CreateUserMessageQueueName = "create-user-flow:create-user-message-queue";
    public const string CreateInformingContactMessageQueueName = "create-user-flow:create-informing-contact-message-queue";
    public const string CreateFinancialAccountMessageQueueName = "create-user-flow:create-financial-account-message-queue";
    public const string CreateCustomerMessageQueueName = "create-user-flow:create-customer-message-queue";
    #endregion

    #region order-flow
    public const string MarkBasketAsPaidMessageQueueName = "order-flow:mark-basket-as-paid-message-queue";
    public const string BasketMarkedAsPaidEventQueueName = "order-flow:basket-marked-as-paid-event-queue";
    public const string InitiateOrderMessageQueueName = "order-flow:initiate-order-message-queue";
    public const string OrderInitiatedEventQueueName = "order-flow:order-initiated-event-queue";
    public const string MarkOrderAsPaidMessageQueueName = "order-flow:mark-order-as-paid-message-queue";
    public const string OrderMarkedAsPaidEventQueueName = "order-flow:order-marked-as-paid-event-queue";
    public const string MarkOrderAsFailedMessageQueueName = "order-flow:mark-order-as-failed-message-queue";
    public const string OrderMarkedAsFailedEventQueueName = "order-flow:order-marked-as-failed-event-queue";
    public const string MarkOrderAsRefundedMessageQueueName = "order-flow:mark-order-as-refunded-message-queue";
    public const string OrderMarkedAsRefundedEventQueueName = "order-flow:order-marked-as-refunded-event-queue";
    public const string MarkOrderAsReversedMessageQueueName = "order-flow:mark-order-as-reversed-message-queue";
    public const string OrderMarkedAsReversedEventQueueName = "order-flow:order-marked-as-reversed-event-queue";
    public const string MarkOrderAsDeliveredMessageQueueName = "order-flow:mark-order-as-delivered-message-queue";
    public const string AddStockMessageQueueName = "order-flow:add-stock-message-queue";
    public const string RemoveStockMessageQueueName = "order-flow:remove-stock-message-queue";
    #endregion

    #region create-contract-flow
    public const string ContractCreatedEventQueueName = "create-contract-flow:contract-created-event-queue";
    public const string FinalizeCreateContractMessageQueueName = "create-contract-flow:finalize-create-contract-message-queue";
    #endregion

    #region identity
    public const string CreateCustomerGroupForClientIdMessageQueueName = "identity:create-customer-group-for-clientId-message-queue";
    public const string SendSignUpVerificationCodeMessageQueueName = "identity:send-signup-verification-code-message-queue";
    #endregion

    #region Kernel
    public const string InformingBaseParameterUpdatedEventQueueName = "kernel:informing-baseparameter-updated-event-queue";
    public const string MessagingBaseParameterUpdatedEventQueueName = "kernel:messaging-baseparameter-updated-event-queue";
    public const string FinancialBaseParameterUpdatedEventQueueName = "kernel:financial-baseparameter-updated-event-queue";
    public const string CustomerClubBaseParameterUpdatedEventQueueName = "kernel:customer-club-baseparameter-updated-event-queue";
    public const string CustomerBaseParameterUpdatedEventQueueName = "kernel:customer-baseparameter-updated-event-queue";
    public const string CatalogBaseParameterUpdatedEventQueueName = "kernel:catalog-baseparameter-updated-event-queue";
    public const string BasketBaseParameterUpdatedEventQueueName = "kernel:basket-baseparameter-updated-event-queue";
    public const string OrderBaseParameterUpdatedEventQueueName = "kernel:order-baseparameter-updated-event-queue";
    public const string ContractBaseParameterUpdatedEventQueueName = "kernel:contract-baseparameter-updated-event-queue";
    public const string BlockChainBaseParameterUpdatedEventQueueName = "kernel:blockchain-baseparameter-updated-event-queue";
    public const string BlockChainLogProcessorBaseParameterUpdatedEventQueueName = "kernel:blockchain-log-processor-baseparameter-updated-event-queue";
    public const string BlockChainTransferBaseParameterUpdatedEventQueueName = "kernel:blockchain-transfer-baseparameter-updated-event-queue";
    public const string BlockChainPaymentBaseParameterUpdatedEventQueueName = "kernel:blockchain-payment-baseparameter-updated-event-queue";
    public const string BlockChainIdentityBaseParameterUpdatedEventQueueName = "kernel:blockchain-identity-baseparameter-updated-event-queue";
    public const string GameBaseParameterUpdatedEventQueueName = "kernel:game-baseparameter-updated-event-queue";
    #endregion

    #region catalog
    public const string AddProductToBasketMessageQueueName = "catalog:add-product-to-basket-message-queue";
    public const string ProductSoldoutEventQueueName = "catalog:product-soldout-event-queue";
    public const string ProductPriceChangedEventQueueName = "catalog:product-price-changed-event-queue";
    public const string ProductUpdatedEventQueueName = "catalog:product-updated-event-queue";
    #endregion

    #region Triggers
    public const string ContractTriggeredQueueName = "trigger:contract-triggered-event-queue";

    #endregion

    #region withdraw flow
    public const string BlockChainTransferMessageQueueName = "withdraw-flow:blockchain-transfer-message-queue";
    public const string BlockChainTransferedEventQueueName = "withdraw-flow:blockchain-transfered-event-queue";
    public const string BlockChainTransferFailedEventQueueName = "withdraw-flow:blockchain-transfer-failed-event-queue";
    #endregion

    #region Block Chain
    public const string CreateLogProcessorErc721TokenMessageQueueName = "blockchain:create-log-processor-erc721-token-message-queue";
    public const string UpdateLogProcessorErc721TokenMessageQueueName = "blockchain:update-log-processor-erc721-token-message-queue";
    public const string DeleteLogProcessorErc721TokenMessageQueueName = "blockchain:delete-log-processor-erc721-token-message-queue";

    public const string CreateTransferMainTokenMessageQueueName = "blockchain:create-transfer-token-message-queue";
    public const string UpdateTransferMainTokenMessageQueueName = "blockchain:update-transfer-token-message-queue";
    public const string DeleteTransferMainTokenMessageQueueName = "blockchain:delete-transfer-token-message-queue";
    
    public const string CreatePaymentMainTokenMessageQueueName = "blockchain:create-payment-token-message-queue";
    public const string UpdatePaymentMainTokenMessageQueueName = "blockchain:update-payment-token-message-queue";
    public const string DeletePaymentMainTokenMessageQueueName = "blockchain:delete-payment-token-message-queue";

    public const string CreateCurrencyMainTokenMessageQueueName = "blockchain:create-currency-token-message-queue";
    public const string UpdateCurrencyMainTokenMessageQueueName = "blockchain:update-currency-token-message-queue";
    public const string DeleteCurrencyMainTokenMessageQueueName = "blockchain:delete-currency-token-message-queue";

    public const string CreateLogProcessorMainTokenMessageQueueName = "blockchain:create-log-processor-token-message-queue";
    public const string UpdateLogProcessorMainTokenMessageQueueName = "blockchain:update-log-processor-token-message-queue";
    public const string DeleteLogProcessorMainTokenMessageQueueName = "blockchain:delete-log-processor-token-message-queue";

    public const string CreateTransferErc20TokenMessageQueueName = "blockchain:create-transfer-erc20-token-message-queue";
    public const string UpdateTransferErc20TokenMessageQueueName = "blockchain:update-transfer-erc20-token-message-queue";
    public const string DeleteTransferErc20TokenMessageQueueName = "blockchain:delete-transfer-erc20-token-message-queue";

    public const string CreatePaymentErc20TokenMessageQueueName = "blockchain:create-payment-erc20-token-message-queue";
    public const string UpdatePaymentErc20TokenMessageQueueName = "blockchain:update-payment-erc20-token-message-queue";
    public const string DeletePaymentErc20TokenMessageQueueName = "blockchain:delete-payment-erc20-token-message-queue";

    public const string CreateCurrencyErc20TokenMessageQueueName = "blockchain:create-currency-erc20-token-message-queue";
    public const string UpdateCurrencyErc20TokenMessageQueueName = "blockchain:update-currency-erc20-token-message-queue";
    public const string DeleteCurrencyErc20TokenMessageQueueName = "blockchain:delete-currency-erc20-token-message-queue";

    public const string CreateLogProcessorErc20TokenMessageQueueName = "blockchain:create-log-processor-erc20-token-message-queue";
    public const string UpdateLogProcessorErc20TokenMessageQueueName = "blockchain:update-log-processor-erc20-token-message-queue";
    public const string DeleteLogProcessorErc20TokenMessageQueueName = "blockchain:delete-log-processor-erc20-token-message-queue";

    public const string BlockChainTokenPriceUpdatedEventQueueName = "blockchain:token-price-updated-event-queue";
    #endregion

    #region Block Chain Log Processor


    #endregion

    #region Block Chain Transfer
    public const string CreateTransferWalletMessageQueueName = "blockchain-transfer:create-transfer-wallet-message-queue";
    public const string CreateCloseMatchWalletDocumentMessageQueueName = "blockchain-transfer:create-close-match-wallet-document-message-queue";
    #endregion

    #region create-collection-flow
    public const string CreateCollectionMessageQueueName = "create-collection-flow:create-collection-message-queue";
    public const string CreateCollectionTokenMessageQueueName = "create-collection-flow:create-collection-token-message-queue";
    public const string CreateCollectionSmartContractMessageQueueName = "create-collection-flow:create-collection-smartcontract-message-queue";
    public const string CreateCollectionReferralMessageQueueName = "create-collection-flow:create-collection-referral-message-queue";
    public const string DeleteCollectionMessageQueueName = "create-collection-flow:delete-collection-message-queue";
    public const string DeleteNftTokenMessageQueueName = "create-collection-flow:delete-nft-token-message-queue";
    #endregion

    #region nft-traded-flow
    public const string NftTradedEventQueueName = "nft-traded-flow:nft-traded-event-queue";
    public const string ChangeNftPriceMessageQueueName = "nft-traded-flow:change-nft-price-message-queue";
    public const string AddNftToBasketMessageQueueName = "nft-traded-flow:add-nft-to-basket-message-queue";
    public const string CreateTradeDocumentMessageQueueName = "nft-traded-flow:create-trade-document-message-queue";
    #endregion

    #region nft-staked-flow
    public const string NftStakedEventQueueName = "nft-staked-flow:nft-staked-event-queue";
    public const string ActivateSmartContractReferralMessageQueueName = "nft-staked-flow:activate-smart-contract-referral-message-queue";
    public const string FindParentOfCustomerForNftStakedMessageQueueName = "nft-staked-flow:find-parent-of-customer-message-queue";
    public const string CalculateNftReferralRewardMessageQueueName = "nft-staked-flow:calculate-nft-referral-reward-message-queue";
    public const string CreateNftReferralRewardDocumentMessageQueueName = "nft-traded-flow:create-nft-referral-reward-document-message-queue";
    public const string ActivateStakePlanMessageQueueName = "nft-staked-flow:activate-stake-plan-message-queue";
    #endregion

    #region nft-unStaked-flow
    public const string NftUnStakedEventQueueName = "nft-unstaked-flow:nft-unstaked-event-queue";
    public const string DeActivateSmartContractReferralMessageQueueName = "nft-unstaked-flow:deactivate-smart-contract-referral-message-queue";

    public const string AddNftProductStockQueueName = "nft-unstaked-flow:add-nft-product-stock-message-queue";
    public const string DeActivateStakePlanMessageQueueName = "nft-unstaked-flow:deactivate-stake-plan-message-queue";
    #endregion

    #region Financial
    #endregion

    #region Game
    public const string CreateBetDocumentMessageQueueName = "game:create-bet-document-message-queue";
    public const string NotifyBetInitiatedMessageQueueName = "game:notify-bet-initiated-message-queue";
    public const string NotifyBetConfirmedMessageQueueName = "game:notify-bet-confirmed-message-queue";
    public const string NotifyBetFailedMessageQueueName = "game:notify-bet-failed-message-queue";
    public const string NotifyBetStatusMessageQueueName = "game:notify-bet-status-message-queue";
    public const string CloseMatchWalletMessageQueueName = "game:close-match-wallet-message-queue";
    public const string NotifyRewardPaidMessageQueueName = "game:notify-reward-paid-message-queue";
    public const string NotifyMatchOverviewMessageQueueName = "game:notify-match-overview-message-queue";
    public const string NotifyMatchMinutesLeftMessageQueueName = "game:notify-match-minutes-left-message-queue";

    public const string TransferBetRewardMessageQueueName = "game:transfer-bet-reward-message-queue";
    public const string CreateBetRewardDocumentMessageQueueName = "game:create-bet-reward-document-message-queue";

    public const string TransferBetReferralRewardMessageQueueName = "game:transfer-bet-referral-reward-message-queue";
    public const string CreateBetReferralRewardDocumentMessageQueueName = "game:create-bet-referral-reward-document-message-queue";

    public const string RemoveMatchWalletFromPaymentMessageQueueName = "game:remove-match-wallet-from-payment-message-queue";
    #endregion

    #region initiate-match-flow
    public const string InitiateMatchMessageQueueName = "initiate-match-flow:initiate-match-message-queue";
    public const string CreateMatchWalletMessageQueueName = "initiate-match-flow:create-match-wallet-message-queue";
    public const string CreateMatchAccountMessageQueueName = "initiate-match-flow:create-match-account-message-queue";
    #endregion

    #region BlockProcessor
    public const string BlockProcessorCreateWalletAddressEventQueueName = "blockprocessor:create-wallet-address-event-queue";
    public const string BlockProcessorDeleteWalletAddressEventQueueName = "blockprocessor:delete-wallet-address-event-queue";
    #endregion

    #region LogProcessor
    public const string LogProcessorCreateTokenEventQueueName = "logprocessor:create-token-event-queue";
    public const string LogProcessorDeleteTokenEventQueueName = "logprocessor:delete-token-event-queue";
    #endregion

    #region Transfer Flow
    public const string TransferInitiatedMessageQueueName = "transfer-initiated-message-queue";
    public const string TransferConfirmedMessageQueueName = "transfer-confirmed-message-queue";
    public const string TransferFailedMessageQueueName = "transfer-failed-message-queue";
    public const string NotifyTransferConfirmedMessageQueueName = "notify-transfer-confirmed-message-queue";
    #endregion
}