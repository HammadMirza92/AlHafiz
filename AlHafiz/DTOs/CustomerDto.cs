namespace AlHafiz.DTOs
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class CreateCustomerDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }
    }

    public class UpdateCustomerDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
