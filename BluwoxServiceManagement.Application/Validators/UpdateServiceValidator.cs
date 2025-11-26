// Path: src/BluwoxServiceManagement.Application/Validators/UpdateServiceValidator.cs

using BluwoxServiceManagement.Application.DTOs.Request;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BluwoxServiceManagement.Application.Validators;

public class UpdateServiceValidator : AbstractValidator<UpdateServiceRequest>
{
    public UpdateServiceValidator()
    {
        RuleFor(x => x.ServiceName)
            .NotEmpty().WithMessage("Service name is required")
            .MinimumLength(2).WithMessage("Service name must be at least 2 characters")
            .MaximumLength(200).WithMessage("Service name cannot exceed 200 characters")
            .Must(BeValidServiceName).WithMessage("Service name can only contain letters, numbers, spaces, hyphens, and underscores");

        RuleFor(x => x.BaseFare)
            .NotEmpty().WithMessage("Base fare is required")
            .GreaterThan(0).WithMessage("Base fare must be greater than 0")
            .LessThanOrEqualTo(10000000).WithMessage("Base fare cannot exceed 10,000,000")
            .Must(baseFare => DecimalPlaces(baseFare) <= 2)
            .WithMessage("Base fare can have at most 2 decimal places");

        RuleFor(x => x.CategoryIds)
            .NotEmpty().WithMessage("At least one category must be selected")
            .Must(list => list.Count > 0).WithMessage("At least one category must be selected")
            .Must(list => list.Count <= 10).WithMessage("Cannot assign more than 10 categories to a service");
    }

    private bool BeValidServiceName(string serviceName)
    {
        if (string.IsNullOrWhiteSpace(serviceName))
            return false;

        var regex = new Regex(@"^[a-zA-Z0-9\s\-_]+$");
        return regex.IsMatch(serviceName);
    }

    private static int DecimalPlaces(decimal value)
    {
        value = Math.Abs(value); // Ensure positive
        int[] bits = decimal.GetBits(value);
        int exponent = (bits[3] >> 16) & 0xFF;
        return exponent;
    }
}