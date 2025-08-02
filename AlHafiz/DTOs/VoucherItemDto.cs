namespace AlHafiz.DTOs
{
    public class VoucherItemDto
    {
        public int Id { get; set; }
        public int VoucherId { get; set; }
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public decimal Weight { get; set; }
        public decimal Kat { get; set; }
        public decimal NetWeight { get; set; }
        public decimal DesiMan { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public bool IsTrackStock { get; set; } = true;
    }

    public class CreateVoucherItemDto
    {
        public int ItemId { get; set; }
        public decimal Weight { get; set; }
        public decimal Kat { get; set; }
        public decimal NetWeight { get; set; }
        public decimal DesiMan { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public bool isTrackStock { get; set; }
    }

    public class UpdateVoucherItemDto
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public decimal Weight { get; set; }
        public decimal Kat { get; set; }
        public decimal NetWeight { get; set; }
        public decimal DesiMan { get; set; }
        public decimal Rate { get; set; }
        public decimal Amount { get; set; }
        public bool IsTrackStock { get; set; } = true;
    }
}
