import { AddTransactionDialog } from "@/components/transaction/add-transaction-dialog";
import TransactionTable from "@/components/transaction/transaction-table";
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import { fetchCategories } from "@/lib/data/categories-data";
import { fetchTransactions } from "@/lib/data/transaction-data";
import { fetchWallets } from "@/lib/data/wallet-data";

export default async function Transactions() {
    const transactions = await fetchTransactions();
    const categories = await fetchCategories();
    const wallets = await fetchWallets();
    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Manage your transactions for income and expenses.
                    </p>
                </div>
                <div className="flex gap-2">
                    <AddTransactionDialog />
                </div>
            </div>

            <Card>
                <CardHeader>
                    <CardTitle>Personal Transactions</CardTitle>
                    <CardDescription>
                        Your personal transaction history of income and
                        expenses.
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    <TransactionTable
                        transactions={transactions}
                        categories={categories}
                        wallets={wallets}
                    />
                </CardContent>
            </Card>
        </div>
    );
}
