import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import {
    ArrowRightLeft,
    HandCoinsIcon,
    LandmarkIcon,
    PencilIcon,
    SmartphoneIcon,
} from "lucide-react";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { Wallet } from "@/lib/definitions";

const iconMap: Record<string, React.ReactNode> = {
    Cash: <HandCoinsIcon />,
    Bank: <LandmarkIcon />,
    MFS: <SmartphoneIcon />,
};

export default function WalletCard({ wallet }: { wallet: Wallet }) {
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
                <Button variant="ghost" size="sm">
                    <PencilIcon />
                    <span className="sr-only">Edit</span>
                </Button>
            </CardHeader>
            <CardContent>
                <div className="text-2xl font-bold mt-2">
                    {`${wallet.balance.toFixed(2)} ${wallet.currency}`}
                </div>
                <div className="mt-6">
                    <Button
                        className="w-full flex items-center justify-center gap-2"
                        variant={"outline"}
                    >
                        <ArrowRightLeft className="h-4 w-4" />
                        Transfer
                    </Button>
                </div>
            </CardContent>
        </Card>
    );
}
