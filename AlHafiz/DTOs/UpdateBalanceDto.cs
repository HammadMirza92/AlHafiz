using AlHafiz.Enums;

namespace AlHafiz.DTOs
{
    public class UpdateBalanceDto
    {
        public int CustomerId { get; set; }
        public PaymentType PaymentType { get; set; }
        public decimal AmountSpent { get; set; }
    }

}
