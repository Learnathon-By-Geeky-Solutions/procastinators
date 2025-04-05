using FinanceTracker.Application.Extensions;
using FluentValidation;

namespace FinanceTracker.Application.PersonalTransactions.Commands.UpdatePersonalTransaction;

public class UpdatePersonalTransactionCommandValidator
    : AbstractValidator<UpdatePersonalTransactionCommand>
{
    public UpdatePersonalTransactionCommandValidator()
    {
        RuleFor(x => x.TransactionType).MustBeValidTransactionType();
        RuleFor(x => x.Amount).GreaterThan(0);
        RuleFor(x => x.Timestamp).NotEmpty();
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.WalletId).NotEmpty();
    }
}
