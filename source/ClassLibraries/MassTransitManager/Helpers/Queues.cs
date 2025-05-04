namespace MassTransitManager.Helpers;

public static class Queues
{
    #region General
    public const string UpdateInformingContactMessageQueueName = "update-informing-contact-message-queue";
    public const string SendGeneralVerificationCodeMessageQueueName = "send-general-verification-code-message-queue";
    public const string CreateResourceMessageQueueName = "create-resource-message-queue";
    public const string SubmitSystemErrorMessageQueueName = "submit-system-error-message-queue";
    #endregion

    #region create-user-flow
    public const string CreateUserMessageQueueName = "create-user-flow:create-user-message-queue";
    public const string CreateInformingContactMessageQueueName = "create-user-flow:create-informing-contact-message-queue";
    #endregion

    #region identity
    public const string CreateCustomerGroupForClientIdMessageQueueName = "identity:create-customer-group-for-clientId-message-queue";
    public const string SendSignUpVerificationCodeMessageQueueName = "identity:send-signup-verification-code-message-queue";
    #endregion

    #region Game
    public const string NotifyBetInitiatedMessageQueueName = "game:notify-bet-initiated-message-queue";
    public const string NotifyBetConfirmedMessageQueueName = "game:notify-bet-confirmed-message-queue";
    public const string NotifyBetFailedMessageQueueName = "game:notify-bet-failed-message-queue";
    public const string NotifyBetStatusMessageQueueName = "game:notify-bet-status-message-queue";
    public const string NotifyRewardPaidMessageQueueName = "game:notify-reward-paid-message-queue";
    public const string NotifyMatchOverviewMessageQueueName = "game:notify-match-overview-message-queue";
    public const string NotifyMatchMinutesLeftMessageQueueName = "game:notify-match-minutes-left-message-queue";
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