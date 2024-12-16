namespace CacheManager.Helpers;

public static class CacheKeys
{
	public const string ConversationSequenceKeyPrefix = "ConversationSequence_", UserOnlineStatusKeyPrefix = "UserOnlineStatus_",
		UserConversationKeyPrefix = "UserConversation_", UserConversationIdKeyPrefix = "UserConversationId_", 
		RegisterUserReferralCodeKeyPrefix = "RegisterUser_", BlockChainIdentitySecret = "BlockChainIdentitySecret",
		NethereumSessionStoragePrefix = "NethereumSessionStorage_", TemporaryBetPrefix = "TemporaryBet_", GameLadderPrefix = "GameLadder_",
		BotBets = "BotBets", MaintenanaceState = "MaintenanceState", BlockProcessorInstancePrefix = "BlockProcessor_Instance_", 
		LogProcessorInstancePrefix = "LogProcessor_Instance_", BotBalances = "BotBalances";



	public static string MatchOverview(string applicationName)
		=> $"Match_Overview_{applicationName}";
	public static string GameLadder(string applicationName, int pageNumber, int pageSize, long? externalTokenId)
		=> $"{GameLadderPrefix}{applicationName}_pageNumber{pageNumber}_pageSize{pageSize}{(externalTokenId is null ? string.Empty : $"_externalTokenId{externalTokenId.Value}")}";
	public static string GameTokens(string applicationName)
		=> $"GameTokens_{applicationName}";
    public static string GameStreaks(string applicationName)
        => $"Game_Streaks_{applicationName}";

    public static string BlockProcessorInstance(string instanceName)
        => $"{BlockProcessorInstancePrefix}{instanceName}";
    public static string LogProcessorInstance(string instanceName)
        => $"{LogProcessorInstancePrefix}{instanceName}";
}