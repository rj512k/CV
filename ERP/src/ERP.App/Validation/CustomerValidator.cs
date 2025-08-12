using ERP.Domain.Entities;
using FluentValidation;

namespace ERP.App.Validation;

public class CustomerValidator : AbstractValidator<Customer>
{
    public CustomerValidator()
    {
        RuleFor(c => c.Name).NotEmpty().MaximumLength(100);
        RuleFor(c => c.Email).EmailAddress().When(c => !string.IsNullOrWhiteSpace(c.Email));
        RuleFor(c => c.Phone).MaximumLength(30).When(c => !string.IsNullOrWhiteSpace(c.Phone));
    }
}