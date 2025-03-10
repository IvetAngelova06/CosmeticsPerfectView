namespace CosmeticsPerfectView.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User Users { get; set; }
        public int ProductsId { get; set; }
        public Product Products { get; set; }
        public int OrderQuantity { get; set; }
        public string Description { get; set; }
    }
}