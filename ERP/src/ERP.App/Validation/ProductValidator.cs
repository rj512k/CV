using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.App.Validation;

public class ProductValidator : AbstractValidator<Product>
{
    public ProductValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MaximumLength(100);
        RuleFor(p => p.Sku).NotEmpty().MaximumLength(50);
        RuleFor(p => p.Price).GreaterThanOrEqualTo(0);
        RuleFor(p => p.StockQuantity).GreaterThanOrEqualTo(0);
    }
}