namespace OnlineStore.Domain.Entites;
public class Product
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int InventoryCount { get; set; }
    public decimal Price { get; set; }
    public double Discount { get; set; }
}
