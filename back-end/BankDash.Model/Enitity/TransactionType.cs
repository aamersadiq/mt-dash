using System.ComponentModel;

namespace BankDash.Model.Enitity
{
    public enum TransactionType
    {
        [Description("Credit")]
        Credit,
        [Description("Debit")]
        Debit
    }

}
