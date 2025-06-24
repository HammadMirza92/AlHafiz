namespace AlHafiz.DTOs
{
    public class ExpenseHeadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateExpenseHeadDto
    {
        public string Name { get; set; }
    }

    public class UpdateExpenseHeadDto
    {
        public string Name { get; set; }
    }
}
