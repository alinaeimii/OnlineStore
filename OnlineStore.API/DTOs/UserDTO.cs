﻿namespace OnlineStore.API.DTOs;
public class UserDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<OrderDTO> Orders { get; set; }
}