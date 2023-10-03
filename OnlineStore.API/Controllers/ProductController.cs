using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using OnlineStore.API.DTOs;
using OnlineStore.Domain.Entites;
using OnlineStore.Domain.Interfaces;

namespace OnlineStore.API.Controllers;
public class ProductController : BaseController
{
    [HttpPost()]
    public async Task<IActionResult> AddProduct(IRepository<Product> productRepository, IMapper mapper, [FromBody] ProductDTO productDto)
    {
        var product = mapper.Map<Product>(productDto);
        var result = mapper.Map<ProductDTO>(await productRepository.AddAsync(product));

        return Ok(result);
    }

    [HttpPut("increase-inventory/{productId}/{quantity}")]
    public async Task<IActionResult> UpdateIncreaseInventory(IRepository<Product> productRepository, Guid productId, int quantity)
    {
        var product = await productRepository.GetByIdAsync(f => f.Id == productId);
        if (product == null)
        {
            return NotFound("Product not found.");
        }

        product.InventoryCount += quantity;
        await productRepository.UpdateAsync(product);
        return Ok("Inventory increased successfully.");
    }

    [HttpGet("{productId}")]
    public async Task<IActionResult> GetProductById(IRepository<Product> productRepository, IMapper mapper, IMemoryCache cache, Guid productId)
    {
        if (!cache.TryGetValue($"Product_{productId}", out ProductDTO product))
        {
            var currentProduct = await productRepository.GetByIdAsync(f => f.Id == productId);
            if (currentProduct == null)
            {
                return NotFound("Product not found.");
            }

            product = mapper.Map<ProductDTO>(currentProduct);
            product.Price -= (product.Price * (decimal)product.Discount / 100);
            cache.Set($"Product_{productId}", product, TimeSpan.FromMinutes(10));
        }

        return Ok(product);
    }

    [HttpPost("buy/{userId}/{productId}")]
    public async Task<IActionResult> BuyProduct(IRepository<Product> productRepository, IRepository<User> userRepository, IRepository<Order> orderRepository, Guid userId, Guid productId)
    {
        var user = await userRepository.GetByIdAsync(f => f.Id == userId);
        var product = await productRepository.GetByIdAsync(f => f.Id == productId);

        if (user == null || product == null)
        {
            return NotFound("User or product not found.");
        }

        if (product.InventoryCount == 0)
        {
            return BadRequest("Product is out of stock.");
        }

        await orderRepository.AddAsync(new Order
        {
            Product = product,
            Buyer = user
        });

        product.InventoryCount--;
        await productRepository.UpdateAsync(product);

        return Ok("Product bought successfully.");
    }
}