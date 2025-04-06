using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public Guid SellerId { get; set; }

        public int CategoryId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public IFormFile UploadImage { get; set; }

        public string Image { get; set; }

        public decimal Price { get; set; }

        public int Stock { get; set; }

        public bool Deleted { get; set; } = false;

        public Seller Seller;

        public Category Category;
    }
}