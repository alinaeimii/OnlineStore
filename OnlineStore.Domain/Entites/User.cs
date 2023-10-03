﻿namespace OnlineStore.Domain.Entites;
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public virtual ICollection<Order> Orders { get; set; }
}
