namespace BankDash.Model.Dto
{
    public class Transfer
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
    }

}
