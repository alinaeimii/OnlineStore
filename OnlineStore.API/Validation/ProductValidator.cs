using FluentValidation;
using OnlineStore.API.DTOs;
using OnlineStore.Domain.Entites;
using OnlineStore.Domain.Interfaces;

namespace OnlineStore.API.Validation
{
    public class ProductValidator : AbstractValidator<ProductDTO>
    {
        public ProductValidator(IRepository<Product> _productRepository)
        {
            RuleFor(p => p.Id).Empty();
            RuleFor(p => p.Title).NotEmpty()
                .Length(1, 40).WithMessage("Product title must be less than 40 characters.")
                .Must(title => _productRepository.IsExistAsync(f => f.Title.Contains(title)).Result == false).WithMessage("Product title must be unique."); ;
        }
    }
}
