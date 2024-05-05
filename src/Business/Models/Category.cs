
namespace Business.Models
{
    public class Category: Entity
    {
        public string Name { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}
