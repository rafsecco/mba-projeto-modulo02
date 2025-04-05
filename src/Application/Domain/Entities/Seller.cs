namespace Application.Domain.Entities
{
    public class Seller
    {
        public Guid UserId { get; set; }
        public bool Deleted { get; set; } = false;

        public List<Product>? Products;
    }
}