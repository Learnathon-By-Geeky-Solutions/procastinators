namespace FinanceTracker.Domain.Constants.Wallet;

public static class Currencies
{
    public const string BDT = "BDT";
    public const string USD = "USD";
    public static List<string> GetAll()
    {
        return [BDT, USD];
    }
}
