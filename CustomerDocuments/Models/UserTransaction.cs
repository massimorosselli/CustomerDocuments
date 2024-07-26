namespace CustomerDocuments.Models
{
    public class UserTransaction
    {
        public int UserTransactionId { get; set; }
        public int UserId { get; set; }
        public int DocumentId { get; set; }
        public DateTime? TransactionDate { get; set; }
    }
}
