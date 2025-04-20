import { Badge } from "@/components/ui/badge";
import { Category, Transaction, Wallet } from "@/lib/definitions";
import {
    ArrowDownLeft,
    ArrowUpRight,
    Coffee,
    ShoppingBag,
    Home,
    Car,
    Utensils,
} from "lucide-react";

export function RecentTransactions({
    transactions,
    wallets,
    categories,
}: {
    readonly transactions: Transaction[];
    readonly wallets: Wallet[];
    readonly categories: Category[];
}) {
    return (
        <div className="space-y-4">
            {transactions.map((transaction) => {
                const category = categories.find(
                    (category) => category.id === transaction.categoryId
                );
                const wallet = wallets.find(
                    (wallet) => wallet.id === transaction.walletId
                );
                return (
                    <div
                        key={transaction.id}
                        className="flex items-center justify-between"
                    >
                        <div className="flex items-center gap-4">
                            <div
                                className={`rounded-full p-2 ${
                                    transaction.transactionType === "Income"
                                        ? "bg-emerald-100"
                                        : "bg-rose-100"
                                }`}
                            >
                                {transaction.transactionType === "Income" ? (
                                    <ArrowUpRight className="h-4 w-4 text-emerald-500" />
                                ) : (
                                    <ArrowDownLeft className="h-4 w-4 text-rose-500" />
                                )}
                            </div>
                            <div>
                                <p className="text-sm font-medium leading-none">
                                    {category?.title ?? "Deleted Category"}
                                </p>
                                <div className="flex items-center gap-2 mt-1">
                                    <Badge
                                        variant="outline"
                                        className="text-xs font-normal"
                                    >
                                        {wallet?.name ?? "Deleted Wallet"}
                                    </Badge>
                                </div>
                            </div>
                        </div>
                        <div className="text-right">
                            <p
                                className={`text-md font-medium ${
                                    transaction.transactionType === "Income"
                                        ? "text-emerald-500"
                                        : "text-rose-500"
                                }`}
                            >
                                {`${
                                    transaction.transactionType === "Income"
                                        ? "+"
                                        : "-"
                                }${transaction.amount.toFixed(2)} BDT`}
                            </p>
                            <p className="text-xs text-muted-foreground mt-1">
                                {new Date(transaction.timestamp).toLocaleString(
                                    "en-US",
                                    {
                                        hour: "2-digit",
                                        minute: "2-digit",
                                        hour12: true,
                                        day: "2-digit",
                                        month: "short",
                                        year: "numeric",
                                    }
                                )}
                            </p>
                        </div>
                    </div>
                );
            })}
        </div>
    );
}
