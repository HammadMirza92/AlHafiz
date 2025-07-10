namespace AlHafiz.DTOs
{
    public class CustomerItemRateDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateCustomerItemRateDto
    {
        public int CustomerId { get; set; }
        public int ItemId { get; set; }
        public decimal Rate { get; set; }
    }

    public class UpdateCustomerItemRateDto
    {
        public decimal Rate { get; set; }
    }

    public class CustomerRateDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public List<ItemRateDto> ItemRates { get; set; } = new List<ItemRateDto>();
    }

    public class ItemRateDto
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal? Rate { get; set; }
        public bool HasRate { get; set; }
    }

    public class RateMatrixDto
    {
        public List<CustomerRateDto> CustomerRates { get; set; } = new List<CustomerRateDto>();
        public List<ItemDto> Items { get; set; } = new List<ItemDto>();
        public List<CustomerDto> Customers { get; set; } = new List<CustomerDto>();
    }

    public class BulkRateUpdateDto
    {
        public List<SetRateDto> Rates { get; set; } = new List<SetRateDto>();
    }

    public class RateHistoryDto
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal OldRate { get; set; }
        public decimal NewRate { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}