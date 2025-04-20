import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import { fetchTransactionReport } from "@/lib/data/transaction-data";
import { fetchTotalBalance } from "@/lib/data/wallet-data";
import { ArrowDownRight, ArrowUpRight, Wallet } from "lucide-react";
import Link from "next/link";

export default async function Dashboard() {
    const totalBalance = await fetchTotalBalance();
    const { grandTotal: totalIncome } = await fetchTransactionReport(
        "income",
        30
    );
    const { categories, grandTotal: totalExpenses } =
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
                            Income
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
                            Expenses
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

            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-7 mt-4">
                <Card className="col-span-3">
                    <CardHeader>
                        <CardTitle>Expense Summary</CardTitle>
                        <CardDescription>Breakdown by category</CardDescription>
                    </CardHeader>
                    <CardContent>{/* <ExpenseSummary /> */}</CardContent>
                </Card>
                <Card className="col-span-4">
                    <CardHeader>
                        <CardTitle>Recent Transactions</CardTitle>
                    </CardHeader>
                    <CardContent>{/* <RecentTransactions /> */}</CardContent>
                </Card>
            </div>
        </div>
    );
}
