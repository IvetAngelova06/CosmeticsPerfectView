using System.ComponentModel.DataAnnotations.Schema;

namespace CosmeticsPerfectView.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int CategoriesId {  get; set; }
        public Category Categories { get; set; }
        public int ProductTypeId {  get; set; }
        public ProductType ProductTypes { get; set; }
        public string URLimage {  get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }
        public decimal PromoPercent { get; set; }
        public string Description { get; set; }
        public DateTime DateRegister { get; set; } = DateTime.Now;
        public ICollection<Order> Orders { get; set; }

    }
}
