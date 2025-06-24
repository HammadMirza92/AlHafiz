namespace AlHafiz.DTOs
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class CreateItemDto
    {
        public string Name { get; set; }
    }

    public class UpdateItemDto
    {
        public string Name { get; set; }
    }
}
