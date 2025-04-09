import { Button } from "@/components/ui/button";
import { Card } from "@/components/ui/card";
import { fetchWallets } from "@/lib/data/wallet-data";
import WalletCard from "@/components/wallet/wallet-card";
import { PlusIcon } from "lucide-react";

export default async function Wallets() {
    const wallets = await fetchWallets();

    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <p className="text-muted-foreground">Manage your wallets</p>

            <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-4 3xl:grid-cols-8">
                <Card className="flex flex-col items-center justify-center p-6">
                    <Button
                        variant="outline"
                        className="h-12 w-12 rounded-full"
                    >
                        <PlusIcon className="h-6 w-6" />
                    </Button>
                    <p className="mt-2 font-medium">Add New Wallet</p>
                </Card>
                {wallets.map((wallet) => (
                    <WalletCard wallet={wallet} key={wallet.id} />
                ))}
            </div>
        </div>
    );
}
