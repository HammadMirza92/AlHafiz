using AlHafiz.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlHafiz.Models
{
    public class CashTransaction
    {
        [Key]
        public int Id { get; set; }

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

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        [Required]
        public bool IsCashReceived { get; set; }

        [MaxLength(500)]
        public string Details { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }


    }
}
