
namespace FinanceTracker.Domain.Constants.Category;

public static class TransactionTypes
{
    public const string Expense = "Expense";
    public const string Income = "Income";

    public static List<string> GetAll()
    {
        return [Expense, Income];
    }
}
