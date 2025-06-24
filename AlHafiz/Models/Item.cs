using System.ComponentModel.DataAnnotations;

namespace AlHafiz.Models
{
    public class Item
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual ICollection<VoucherItem> VoucherItems { get; set; }
        public virtual ICollection<Stock> Stocks { get; set; }
    }
}
