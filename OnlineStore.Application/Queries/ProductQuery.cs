using AutoMapper;
using MediatR;
using Microsoft.Extensions.Caching.Memory;
using OnlineStore.Application.DTOs;
using OnlineStore.Application.Helper;
using OnlineStore.Domain.Entites;
using OnlineStore.Domain.Interfaces;

namespace OnlineStore.Application.Queries
{
    public class ProductQuery
    {
        public record GetProductByIdQuery(Guid ProductId) : IRequest<ApiResponse<ProductDTO>>;

        private class GetProductByIdHandler : IRequestHandler<GetProductByIdQuery, ApiResponse<ProductDTO>>
        {
            private readonly IRepository<Product> _productRepository;
            private readonly IMemoryCache _cache;
            private readonly IMapper _mapper;
            public GetProductByIdHandler(IRepository<Product> productRepository, IMapper mapper, IMemoryCache cache)
            {
                _productRepository = productRepository;
                _cache = cache;
                _mapper = mapper;
            }
            public async Task<ApiResponse<ProductDTO>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
            {

                var apiResponse = new ApiResponse<ProductDTO>();
                if (!_cache.TryGetValue($"Product_{request.ProductId}", out ProductDTO cachedProduct))
                {
                    var product = await _productRepository.GetByIdAsync(f => f.Id == request.ProductId);
                    if (product == null)
                    {
                        apiResponse.Data = null;
                        apiResponse.StatusCode = 404;
                    }
                    else
                    {
                        product.Price += (product.Price * (decimal)product.Discount / 100);
                        await _productRepository.UpdateAsync(product);
                        var productDTO = _mapper.Map<ProductDTO>(product);
                        _cache.Set($"Product_{request.ProductId}", productDTO, TimeSpan.FromMinutes(10));
                        apiResponse.Data = productDTO;
                        apiResponse.StatusCode = 200;
                    }
                }
                else
                {
                    apiResponse.Data = cachedProduct;
                    apiResponse.StatusCode = 200;
                }
                return apiResponse;

            }
        }

    }
}
