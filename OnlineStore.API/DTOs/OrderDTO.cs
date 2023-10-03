namespace OnlineStore.API.DTOs;
public class OrderDTO
{
    public Guid Id { get; set; }
    public ProductDTO Product { get; set; }
    public DateTime CreationDate { get; set; }
    public UserDTO Buyer { get; set; }
}

