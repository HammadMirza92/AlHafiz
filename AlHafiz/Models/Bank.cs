using System.ComponentModel.DataAnnotations;

namespace AlHafiz.Models
{
    public class Bank
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<Voucher> Vouchers { get; set; }
        public virtual ICollection<CashTransaction> CashTransactions { get; set; }
    }
}
