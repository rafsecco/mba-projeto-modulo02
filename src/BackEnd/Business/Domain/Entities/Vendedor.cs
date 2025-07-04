namespace Business.Domain.Entities
{
    public class Vendedor
    {
        public Guid UserId { get; set; }

        public List<Produto>? Produtos;
    }
}