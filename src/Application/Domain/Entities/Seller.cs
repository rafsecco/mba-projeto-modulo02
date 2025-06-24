namespace Core.Domain.Entities
{
    public class Seller
    {
        public Guid UserId { get; set; }

        public List<Produto>? Products;
    }
}