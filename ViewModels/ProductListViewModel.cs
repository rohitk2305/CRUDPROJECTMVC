using System.Collections.Generic;
using ProjectCRUDOperation.Models;



namespace ProjectCRUDOperation.ViewModels

{
    public class ProductListViewModel
    {
        public List<Product> Products { get; set; } // List of products to display.
        public int CurrentPage { get; set; } // Current page number for pagination.
        public int TotalPages { get; set; } // Total number of pages for pagination.
    }
}
