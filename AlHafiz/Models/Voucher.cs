using AlHafiz.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlHafiz.Models
{
    public class Voucher
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public VoucherType VoucherType { get; set; }

        public int? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }

        [Required]
        public PaymentType PaymentType { get; set; }

        public int? BankId { get; set; }
        [ForeignKey("BankId")]
        public virtual Bank Bank { get; set; }

        [MaxLength(200)]
        public string PaymentDetails { get; set; }

        public int? ExpenseHeadId { get; set; }
        [ForeignKey("ExpenseHeadId")]
        public virtual ExpenseHead ExpenseHead { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal? Amount { get; set; }

        [MaxLength(50)]
        public string? GariNo { get; set; }

        [MaxLength(500)]
        public string Details { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<VoucherItem> VoucherItems { get; set; }
    }
}
