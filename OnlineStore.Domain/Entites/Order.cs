namespace OnlineStore.Domain.Entites;
public class Order
{
    public Guid Id { get; set; }
    public virtual Product Product { get; set; }
    public DateTime CreationDate { get; set; }
    public virtual User Buyer { get; set; }
}
