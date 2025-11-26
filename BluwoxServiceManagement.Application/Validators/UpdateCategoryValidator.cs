using BluwoxServiceManagement.Application.DTOs.Request;
using FluentValidation;
using System.Text.RegularExpressions;

namespace BluwoxServiceManagement.Application.Validators;

public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryRequest>
{
    public UpdateCategoryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Category name is required")
            .MinimumLength(2).WithMessage("Category name must be at least 2 characters")
            .MaximumLength(200).WithMessage("Category name cannot exceed 200 characters")
            .Must(BeValidName).WithMessage("Category name can only contain letters, numbers, spaces, hyphens, and underscores");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters");
    }

    private bool BeValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        var regex = new Regex(@"^[a-zA-Z0-9\s\-_/]+$");
        return regex.IsMatch(name);
    }
}