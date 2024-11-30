namespace ProjectCRUDOperation.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        // Navigation property to relate products
        public ICollection<Product> Products { get; set; }
    }
}
