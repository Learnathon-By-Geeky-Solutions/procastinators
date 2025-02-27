using FluentValidation;

namespace FinanceTracker.Application.Categories.Commands.UpdateCategory;

public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
{
    public UpdateCategoryCommandValidator()
    {
        RuleFor(w => w.Title).NotEmpty();
        RuleFor(w => w.DefaultTransactionType).MustBeValidCategoryType();
    }
}
