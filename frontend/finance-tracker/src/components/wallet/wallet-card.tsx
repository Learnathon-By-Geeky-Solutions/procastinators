import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { HandCoinsIcon, LandmarkIcon, SmartphoneIcon } from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { Wallet } from "@/lib/definitions";
import ManageWalletDropdown from "@/components/wallet/manage-wallet-dropdown";
import { TransferFundDialog } from "@/components/wallet/transfer-fund-dialog";

const iconMap: Record<string, React.ReactNode> = {
    Cash: <HandCoinsIcon />,
    Bank: <LandmarkIcon />,
    MFS: <SmartphoneIcon />,
};

export default function WalletCard({
    wallet,
    otherWallets,
}: {
    readonly wallet: Wallet;
    readonly otherWallets: Wallet[];
}) {
    return (
        <Card key={wallet.id}>
            <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                <CardTitle className="flex-1 text-lg flex items-center gap-2">
                    <div className="rounded-full p-2">
                        {iconMap[wallet.type]}
                    </div>
                    <span>{wallet.name}</span>
                </CardTitle>
                <Badge variant="outline">{wallet.type}</Badge>
                <ManageWalletDropdown wallet={wallet} />
            </CardHeader>
            <CardContent>
                <div className="text-2xl font-bold mt-2">
                    {`${wallet.balance.toFixed(2)} ${wallet.currency}`}
                </div>
                <div className="mt-6">
                    <TransferFundDialog
                        sourceWallet={wallet}
                        destinationWallets={otherWallets}
                    />
                </div>
            </CardContent>
        </Card>
    );
}
