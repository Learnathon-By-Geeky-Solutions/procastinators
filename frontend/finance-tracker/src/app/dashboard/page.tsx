import { ExpenseBreakdown } from "@/components/dashboard/expense-breakdown";
import { RecentTransactions } from "@/components/dashboard/recent-transaction";
import { AddTransactionDialog } from "@/components/transaction/add-transaction-dialog";
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import { fetchCategories } from "@/lib/data/categories-data";
import {
    fetchTransactionReport,
    fetchTransactions,
} from "@/lib/data/transaction-data";
import { fetchWallets } from "@/lib/data/wallet-data";
import { ArrowDownRight, ArrowUpRight, Wallet } from "lucide-react";
import Link from "next/link";

export default async function Dashboard() {
    const categories = await fetchCategories();
    const wallets = await fetchWallets();
    const transactionsAll = await fetchTransactions();
    const transactions = transactionsAll.slice(0, 5);
    const totalBalance = wallets.reduce(
        (acc, wallet) => acc + wallet.balance,
        0
    );
    const { grandTotal: totalIncome } = await fetchTransactionReport(
        "income",
        30
    );
    const { categories: categoryReport, grandTotal: totalExpenses } =
        await fetchTransactionReport("expense", 30);
    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Overview of your financial activities
                    </p>
                </div>
            </div>

            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                <Card>
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                            Total Balance
                        </CardTitle>
                        <Wallet className="h-4 w-4 " />
                    </CardHeader>
                    <CardContent>
                        <div className="text-2xl font-bold">{`${totalBalance} BDT`}</div>
                        <div className="flex gap-2">
                            <p className="text-sm text-muted-foreground mt-2">
                                Across all wallets
                            </p>
                            <Link
                                href="/dashboard/wallets"
                                className="text-sm underline mt-2"
                            >
                                View
                            </Link>
                        </div>
                    </CardContent>
                </Card>
                <Card>
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                            Total Income
                        </CardTitle>
                        <ArrowUpRight className="h-4 w-4 text-emerald-500" />
                    </CardHeader>
                    <CardContent>
                        <div className="text-2xl font-bold">{`${totalIncome} BDT`}</div>
                        <p className="text-sm text-muted-foreground mt-2">
                            Last 30 days
                        </p>
                    </CardContent>
                </Card>
                <Card>
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <CardTitle className="text-sm font-medium">
                            Total Expenses
                        </CardTitle>
                        <ArrowDownRight className="h-4 w-4 text-rose-500" />
                    </CardHeader>
                    <CardContent>
                        <div className="text-2xl font-bold">{`${totalExpenses} BDT`}</div>
                        <p className="text-sm text-muted-foreground mt-2">
                            Last 30 days
                        </p>
                    </CardContent>
                </Card>
            </div>

            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-8 mt-4">
                <Card className="col-span-4">
                    <CardHeader>
                        <div className="flex items-center justify-between space-y-0">
                            <CardTitle className="text-lg font-medium">
                                Recent Transactions
                            </CardTitle>
                            <AddTransactionDialog
                                iconOnly={true}
                                categories={categories}
                                wallets={wallets}
                            />
                        </div>
                    </CardHeader>
                    <CardContent>
                        <RecentTransactions
                            transactions={transactions}
                            categories={categories}
                            wallets={wallets}
                        />
                    </CardContent>
                    <CardFooter>
                        <div className="flex items-center justify-end w-full">
                            <Link
                                href="/dashboard/transactions"
                                className="text-sm underline mt-2"
                            >
                                View All
                            </Link>
                        </div>
                    </CardFooter>
                </Card>
                <Card className="col-span-4">
                    <CardHeader>
                        <CardTitle className="text-lg font-medium">
                            Expense Summary
                        </CardTitle>
                        <CardDescription>
                            Breakdown by category (last 30 days)
                        </CardDescription>
                    </CardHeader>
                    <CardContent>
                        <ExpenseBreakdown report={categoryReport} />
                    </CardContent>
                </Card>
            </div>
        </div>
    );
}
