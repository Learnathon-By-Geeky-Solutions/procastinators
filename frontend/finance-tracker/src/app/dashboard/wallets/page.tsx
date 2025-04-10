import { Card } from "@/components/ui/card";
import { fetchWallets } from "@/lib/data/wallet-data";
import WalletCard from "@/components/wallet/wallet-card";
import { AddWalletDialog } from "@/components/wallet/add-wallet-dialog";

export default async function Wallets() {
    const wallets = await fetchWallets();

    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <p className="text-muted-foreground">Manage your wallets</p>

            <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4 3xl:grid-cols-8">
                <Card className="flex flex-col items-center justify-center p-6">
                    <div className="flex flex-col items-center">
                        <AddWalletDialog />
                        <p className="mt-4 font-medium">Add New Wallet</p>
                    </div>
                </Card>
                {wallets.map((wallet) => (
                    <WalletCard wallet={wallet} key={wallet.id} />
                ))}
            </div>
        </div>
    );
}
