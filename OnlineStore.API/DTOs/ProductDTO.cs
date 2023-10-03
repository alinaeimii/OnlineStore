namespace OnlineStore.API.DTOs;
public class ProductDTO
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public int InventoryCount { get; set; }
    public decimal Price { get; set; }
    public double Discount { get; set; }
}
