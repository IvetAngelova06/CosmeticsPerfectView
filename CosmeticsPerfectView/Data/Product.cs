using System.ComponentModel;

namespace CosmeticsPerfectView.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public Category Categories {  get; set; } 
        public int ProductTypyId { get; set; }
        public ProductType ProductTypes {  get; set; }
        public string URLimage { get; set; }
        public decimal Price { get; set; }
        public double PromoPercent { get; set; }
         public string Description { get; set; }
        public DateTime DateRegister { get; set; }
        public ICollection<Order> Orders { get; set; }
    }
}