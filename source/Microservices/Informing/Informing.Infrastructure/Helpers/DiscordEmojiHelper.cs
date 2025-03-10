namespace Informing.Infrastructure.Helpers;

public static class DiscordEmojiHelper
{
    public static string GetEmoji(EmojiType type)
    {
        return type switch
        {
            EmojiType.CoinHeads => "<:CoinHeads:1292910819226550373>",
            EmojiType.CoinTails => "<:CoinTails:1292910912050561064>",
            EmojiType.RpsRock => "<:RpsRock:1292910950793351178>",
            EmojiType.RpsPaper => "<:RpsPaper:1292911043974004819>",
            EmojiType.RpsScissor => "<:RpsScissor:1292911092972126409>",
            EmojiType.DiceOne => "<:DiceOne:1292916249658921000>",
            EmojiType.DiceTwo => "<:DiceTwo:1292916298447327265>",
            EmojiType.DiceThree => "<:DiceThree:1292916343720513579>",
            EmojiType.DiceFour => "<:DiceFour:1292916387185950861>",
            EmojiType.DiceFive => "<:DiceFive:1292916436141867151>",
            EmojiType.DiceSix => "<:DiceSix:1292916485160697949>",
            _ => string.Empty,
        };
    }
    public static string GetEmoji(string game, string optionTitle)
    {
        if (game.Equals("Coin", StringComparison.InvariantCultureIgnoreCase))
        {
            return GetCoinEmoji(optionTitle);
        }
        else if (game.Equals("RockPaperScissor", StringComparison.InvariantCultureIgnoreCase) || game.Equals("RPS", StringComparison.InvariantCultureIgnoreCase))
        {
            return GetRpsEmoji(optionTitle);
        }
        else if (game.Equals("Dice", StringComparison.InvariantCultureIgnoreCase))
        {
            return GetDiceEmoji(optionTitle);
        }
        else
        {
            return GetGeneralEmoji(optionTitle);
        }
    }

    private static string GetCoinEmoji(string optionTitle)
    {
        if (optionTitle.Contains("heads", StringComparison.InvariantCultureIgnoreCase))
            return "<:CoinHeads:1292910819226550373>";
        if (optionTitle.Contains("tails", StringComparison.InvariantCultureIgnoreCase))
            return "<:CoinTails:1292910912050561064>";
        return string.Empty;
    }

    private static string GetRpsEmoji(string optionTitle)
    {
        if (optionTitle.Contains("rock", StringComparison.InvariantCultureIgnoreCase))
            return "<:RpsRock:1292910950793351178>";
        if (optionTitle.Contains("paper", StringComparison.InvariantCultureIgnoreCase))
            return "<:RpsPaper:1292911043974004819>";
        if (optionTitle.Contains("scissor", StringComparison.InvariantCultureIgnoreCase))
            return "<:RpsScissor:1292911092972126409>";
        return string.Empty;
    }

    private static string GetDiceEmoji(string optionTitle)
    {
        if (optionTitle.Contains("dice", StringComparison.InvariantCultureIgnoreCase) && optionTitle.Contains("1", StringComparison.InvariantCultureIgnoreCase))
            return "<:DiceOne:1292916249658921000>";
        if (optionTitle.Contains("dice", StringComparison.InvariantCultureIgnoreCase) && optionTitle.Contains("2", StringComparison.InvariantCultureIgnoreCase))
            return "<:DiceTwo:1292916298447327265>";
        if (optionTitle.Contains("dice", StringComparison.InvariantCultureIgnoreCase) && optionTitle.Contains("3", StringComparison.InvariantCultureIgnoreCase))
            return "<:DiceThree:1292916343720513579>";
        if (optionTitle.Contains("dice", StringComparison.InvariantCultureIgnoreCase) && optionTitle.Contains("4", StringComparison.InvariantCultureIgnoreCase))
            return "<:DiceFour:1292916387185950861>";
        if (optionTitle.Contains("dice", StringComparison.InvariantCultureIgnoreCase) && optionTitle.Contains("5", StringComparison.InvariantCultureIgnoreCase))
            return "<:DiceFive:1292916436141867151>";
        if (optionTitle.Contains("dice", StringComparison.InvariantCultureIgnoreCase) && optionTitle.Contains("6", StringComparison.InvariantCultureIgnoreCase))
            return "<:DiceSix:1292916485160697949>";
        return string.Empty;
    }

    private static string GetGeneralEmoji(string optionTitle)
    {
        var emojiMappings = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase)
        {
            { "heads", "<:CoinHeads:1292910819226550373>" },
            { "tails", "<:CoinTails:1292910912050561064>" },
            { "rock", "<:RpsRock:1292910950793351178>" },
            { "paper", "<:RpsPaper:1292911043974004819>" },
            { "scissor", "<:RpsScissor:1292911092972126409>" },
            { "dice 1", "<:DiceOne:1292916249658921000>" },
            { "dice 2", "<:DiceTwo:1292916298447327265>" },
            { "dice 3", "<:DiceThree:1292916343720513579>" },
            { "dice 4", "<:DiceFour:1292916387185950861>" },
            { "dice 5", "<:DiceFive:1292916436141867151>" },
            { "dice 6", "<:DiceSix:1292916485160697949>" }
        };

        var mapping = emojiMappings
            .FirstOrDefault(m => optionTitle.Contains(m.Key, StringComparison.InvariantCultureIgnoreCase));

        if (!mapping.Equals(default(KeyValuePair<string, string>)))
        {
            return mapping.Value;
        }

        return string.Empty;
    }
    public static string GetIconEmoji()
    {
        return "<:icon:1294431888743727167>";
    }

    public enum EmojiType
    { CoinHeads, CoinTails, RpsRock, RpsPaper, RpsScissor, DiceOne, DiceTwo, DiceThree, DiceFour, DiceFive, DiceSix }
}
