namespace FinanceTracker.Domain.Constants.Wallet;

public static class Currencies
{
    public const string BDT = "BDT";

    public static List<string> GetAll()
    {
        return [BDT];
    }
}
