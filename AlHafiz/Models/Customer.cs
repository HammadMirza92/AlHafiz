using System.ComponentModel.DataAnnotations;

namespace AlHafiz.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Voucher> Vouchers { get; set; }
        public virtual ICollection<CashTransaction> CashTransactions { get; set; }
    }
}
