using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace AlHafiz.Models
{
    public class VoucherItem
    {
        [Key]
        public int Id { get; set; }

        public int VoucherId { get; set; }
        [ForeignKey("VoucherId")]
        public virtual Voucher Voucher { get; set; }

        public int ItemId { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Weight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Kat { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal NetWeight { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DesiMan { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Rate { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
