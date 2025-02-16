namespace FinanceTracker.Domain.Constants.Wallet;

public static class WalletTypes
{
    public const string Bank = "Bank";
    public const string Cash = "Cash";
    public const string MFS = "MFS";

    public static List<string> GetAll()
    {
        return [Bank, Cash, MFS];
    }
}