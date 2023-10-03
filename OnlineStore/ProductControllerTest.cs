using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using OnlineStore.API.AutoMapper;
using OnlineStore.API.Controllers;
using OnlineStore.API.DTOs;
using OnlineStore.Domain.Entites;
using OnlineStore.Infrastructure.Data;
using OnlineStore.Infrastructure.Repositories;
using Xunit;

namespace OnlineStore.Test;
public class ProductControllerTest
{


    private readonly AppDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly IMemoryCache _memoryCache;

    public ProductControllerTest()
    {
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseInMemoryDatabase("OnlineStoreTest");

        _dbContext = new AppDbContext(builder.Options);
        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new Mapping());
        });
        _mapper = mockMapper.CreateMapper();
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    [Fact]
    public async void Add_Product()
    {
        // Arrange
        var productRepository = new Repository<Product>(_dbContext);
        var productMock = new ProductDTO { Id = Guid.NewGuid(), Discount = 0, InventoryCount = 5, Price = 2000, Title = "test1" };
        var controller = new ProductController();


        // Arrange
        var result = await controller.AddProduct(productRepository, _mapper, productMock);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var product = Assert.IsType<ProductDTO>(okResult.Value);

        // Assert
        Assert.Equal(product.Discount, productMock?.Discount);
        Assert.Equal(product.InventoryCount, productMock?.InventoryCount);
        Assert.Equal(product.Price, productMock?.Price);
        Assert.Equal(product.Title, productMock?.Title);
        Assert.True(productMock?.Id != Guid.Empty);
    }

    [Fact]
    public async void Update_Increas_Inventory()
    {
        // Arrange
        var productRepository = new Repository<Product>(_dbContext);
        var productMock = new Product { Id = Guid.NewGuid(), Discount = 0, InventoryCount = 5, Price = 2000, Title = "test1" };
        var controller = new ProductController();

        // Arrange
        await productRepository.AddAsync(productMock);
        await controller.UpdateIncreaseInventory(productRepository, productMock.Id, 5);
        var newIncreaseInventory = await productRepository.GetByIdAsync(f => f.Id == productMock.Id);

        // Assert
        Assert.Equal(newIncreaseInventory?.InventoryCount, 10);
    }

    [Fact]
    public async void Get_Product_By_Id()
    {
        // Arrange
        var productRepository = new Repository<Product>(_dbContext);
        var productMock = new Product { Id = Guid.NewGuid(), Discount = 25, InventoryCount = 5, Price = 2000, Title = "test1" };
        var controller = new ProductController();

        // Arrange
        await productRepository.AddAsync(productMock);
        var result = await controller.GetProductById(productRepository, _mapper, _memoryCache, productMock.Id);
        var okResult = Assert.IsType<OkObjectResult>(result);
        var product = Assert.IsType<ProductDTO>(okResult.Value);

        // Assert
        Assert.Equal(product.Discount, productMock?.Discount);
        Assert.Equal(product.InventoryCount, productMock?.InventoryCount);
        Assert.Equal(product.Price, productMock?.Price - (productMock?.Price * (decimal)productMock?.Discount / 100));
        Assert.Equal(product.Title, productMock?.Title);
        Assert.Equal(product.Id, productMock?.Id);
    }

}
