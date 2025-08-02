using AlHafiz.Enums;

namespace AlHafiz.DTOs
{
    public class VoucherDto
    {
        public int Id { get; set; }
        public VoucherType VoucherType { get; set; }
        public int? CustomerId { get; set; }
        public string CustomerName { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? BankId { get; set; }
        public string BankName { get; set; }
        public string PaymentDetails { get; set; }
        public int? ExpenseHeadId { get; set; }
        public string ExpenseHeadName { get; set; }
        public decimal? Amount { get; set; }
        public string? GariNo { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<VoucherItemDto> VoucherItems { get; set; }
    }

    public class CreateVoucherDto
    {
        public VoucherType VoucherType { get; set; }
        public int? CustomerId { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? BankId { get; set; }
        public string PaymentDetails { get; set; }
        public int? ExpenseHeadId { get; set; }
        public decimal? Amount { get; set; }
        public string? GariNo { get; set; }
        public string Details { get; set; }
        public string CreatedAt { get; set; }
        public List<CreateVoucherItemDto> VoucherItems { get; set; }
    }

    public class UpdateVoucherDto
    {
        public VoucherType VoucherType { get; set; }
        public int? CustomerId { get; set; }
        public PaymentType PaymentType { get; set; }
        public int? BankId { get; set; }
        public string PaymentDetails { get; set; }
        public int? ExpenseHeadId { get; set; }
        public decimal? Amount { get; set; }
        public string? GariNo { get; set; }
        public string Details { get; set; }
        public string CreatedAt { get; set; }
        public List<UpdateVoucherItemDto> VoucherItems { get; set; }
    }

    public class VoucherFilterDto
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public int? CustomerId { get; set; }
        public int? ExpenseHeadId { get; set; }
        public VoucherType? VoucherType { get; set; }
    }
}
