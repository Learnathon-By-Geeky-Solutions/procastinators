using FinanceTracker.Application.Extensions;
using FluentValidation;

namespace FinanceTracker.Application.PersonalTransactions.Commands.CreatePersonalTransaction;

public class CreatePersonalTransactionCommandValidator : AbstractValidator<CreatePersonalTransactionCommand>
{
    public CreatePersonalTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionType).MustBeValidTransactionType();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.WalletId).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
    }
}