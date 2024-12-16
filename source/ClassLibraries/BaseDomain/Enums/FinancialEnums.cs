using System.ComponentModel;

namespace BaseDomain.Enums;

public class FinancialEnums
{
    public enum PatternValueType
    {
        /// <summary>
        /// It is fixed and must be defined in transaction detail
        /// </summary>
        Fixed,
        /// <summary>
        /// It is dynamic and can be defined by user or external third party
        /// </summary>
        Dynamic,
        /// <summary>
        /// It is percentage of total amount
        /// </summary>
        Percent
    }

    public enum AccountType
    {
        [Description("Company account")]
        Company,
        [Description("User account")]
        User,
        [Description("Third party fee account")]
        ThirdParty,
        [Description("Royalty account")]
        Royalty,
        [Description("Match account")]
        Match
    }


    public enum DocumentState
    {
        [Description("Committed")]
        Committed,

        [Description("Initiated")]
        Initiated,

        [Description("Refunded")]
        Refunded,

        [Description("Reversed")]
        Reversed,

        [Description("Fail")]
        Failed = 99,
    }

    public enum DocumentDebitCredit
    {
        Debit,
        Credit
    }

    public enum PatternValueCalculationType
    {
        /// <summary>
        /// The amount is inside total amount
        /// </summary>
        Included,
        /// <summary>
        /// The amount is added to total amount (ex. VAT)
        /// </summary>
        Excluded
    }

    //public enum TransactionType
    //{
    //    [Description("Deposit")]
    //    Deposit,
    //    [Description("Purchase")]
    //    Purchase,
    //    [Description("Withdraw")]
    //    Withdraw,
    //    [Description("Reverse")]
    //    Reverse,
    //    [Description("Refund")]
    //    Refund,
    //    [Description("Reverse refund")]
    //    ReverseRefund,
    //    [Description("Charge")]
    //    Charge,
    //    [Description("Amendment")]
    //    Amendment
    //}
}