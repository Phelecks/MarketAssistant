namespace BaseDomain.Enums;

public class Meessaging
{
    public enum ActionType
    {
        Normal,
        Deleted,
        Replied,
        Forwarded,
        Edited
    }

    public enum MessageType
    {
        Text,
        Url,
        Voice,
        Video,
        File,
        Location
    }

	public enum ConversationType
	{
		P2P,
		Group,
		Channel
	}
}