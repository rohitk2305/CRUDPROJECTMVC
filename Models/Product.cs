using System.ComponentModel.DataAnnotations;

namespace ProjectCRUDOperation.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }

        // Navigation property to relate to Category
        public Category Category { get; set; }
    }
}
