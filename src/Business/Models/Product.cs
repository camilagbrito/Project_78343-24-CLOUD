﻿namespace Business.Models
{
    public class Product:Entity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public bool IsAvailable { get; set; }
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }

    }
}