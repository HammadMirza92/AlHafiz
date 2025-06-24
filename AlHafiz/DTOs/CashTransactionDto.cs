using AlHafiz.Enums;

namespace AlHafiz.DTOs
{
    public class CashTransactionDto
    {
        public int Id { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public string PaymentDetails { get; set; }
        public decimal Amount { get; set; }
        public bool IsCashReceived { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class CreateCashTransactionDto
    {
        public int? CustomerId { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? BankId { get; set; }
        public string PaymentDetails { get; set; }
        public decimal Amount { get; set; }
        public bool IsCashReceived { get; set; }
        public string Details { get; set; }
    }

    public class CashTransactionFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsCashReceived { get; set; }
    }
}
