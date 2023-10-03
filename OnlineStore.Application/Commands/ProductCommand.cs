using AutoMapper;
using MediatR;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Helper;
using OnlineStore.Domain.Entites;
using OnlineStore.Domain.Interfaces;

namespace OnlineStore.Application.Commands;
public class ProductCommand
{
    public record AddCommand(ProductDTO ProductItem) : IRequest<ApiResponse<ProductDTO>>;
    public record UpdateInventoryCountByIdCommand(Guid ProductId, int Quantity) : IRequest<ApiResponse<string>>;
    public record BuyCommand(Guid UserId, Guid ProductId) : IRequest<ApiResponse<string>>;

    private class AddHandler : IRequestHandler<AddCommand, ApiResponse<ProductDTO>>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        public AddHandler(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<ProductDTO>> Handle(AddCommand request, CancellationToken cancellationToken)
        {
            return new ApiResponse<ProductDTO> { Data = _mapper.Map<ProductDTO>(await _productRepository.AddAsync(_mapper.Map<Product>(request.ProductItem))), StatusCode = 201 };
        }
    }
    private class UpdateInventoryCountByIdHandler : IRequestHandler<UpdateInventoryCountByIdCommand, ApiResponse<string>>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IMapper _mapper;
        public UpdateInventoryCountByIdHandler(IRepository<Product> productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<string>> Handle(UpdateInventoryCountByIdCommand request, CancellationToken cancellationToken)
        {
            var apiResponse = new ApiResponse<string>();
            var product = await _productRepository.GetByIdAsync(f => f.Id == request.ProductId);
            if (product == null)
            {
                apiResponse.Data = string.Empty;
                apiResponse.StatusCode = 404;
            }
            else
            {
                product.InventoryCount += request.Quantity;
                await _productRepository.UpdateAsync(product);
                apiResponse.Data = "Inventory increased successfully.";
                apiResponse.StatusCode = 202;
            }
            return apiResponse;

        }
    }
    private class BuyHandler : IRequestHandler<BuyCommand, ApiResponse<string>>
    {
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IMapper _mapper;
        public BuyHandler(IRepository<Product> productRepository, IRepository<User> userRepository, IRepository<Order> orderRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }
        public async Task<ApiResponse<string>> Handle(BuyCommand request, CancellationToken cancellationToken)
        {
            var apiResponse = new ApiResponse<string>();
            var user = await _userRepository.GetByIdAsync(f => f.Id == request.UserId);
            var product = await _productRepository.GetByIdAsync(f => f.Id == request.ProductId);

            if (user == null || product == null)
            {
                apiResponse.Data = "User or product not found.";
                apiResponse.StatusCode = 404;
            }
            else if (product.InventoryCount == 0)
            {
                apiResponse.Data = "Product is out of stock.";
                apiResponse.StatusCode = 400;
            }
            else
            {
                await _orderRepository.AddAsync(new Order
                {
                    Product = product,
                    Buyer = user
                });

                product.InventoryCount--;
                await _productRepository.UpdateAsync(product);
                apiResponse.Data = "Product bought successfully.";
                apiResponse.StatusCode = 200;
            }

            return apiResponse;
        }
    }
}
