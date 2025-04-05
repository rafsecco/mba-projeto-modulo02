namespace Application.Domain.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Deleted { get; set; } = false;

        public List<Product> Products { get; set; }
    }
}