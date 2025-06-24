namespace AlHafiz.DTOs
{
    public class BankDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateBankDto
    {
        public string Name { get; set; }
    }

    public class UpdateBankDto
    {
        public string Name { get; set; }
    }
}
