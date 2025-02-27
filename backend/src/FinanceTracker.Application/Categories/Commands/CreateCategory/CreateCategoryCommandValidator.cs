using FluentValidation;

namespace FinanceTracker.Application.Categories.Commands.CreateCategory;

public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
{
   public CreateCategoryCommandValidator()
   {
        RuleFor(w => w.Title).NotEmpty();
        RuleFor(w => w.DefaultTransactionType).MustBeValidCategoryType();
   }
}
