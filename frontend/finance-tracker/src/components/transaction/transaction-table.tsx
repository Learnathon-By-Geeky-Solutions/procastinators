import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Category, Transaction, Wallet } from "@/lib/definitions";
import { EditCategoryDialog } from "@/components/category/edit-category-dialog";
import { DeleteCategoryDialog } from "@/components/category/delete-category-dialog";

export default function TransactionTable({
    transactions,
    wallets,
    categories,
}: {
    readonly transactions: Transaction[];
    readonly wallets: Wallet[];
    readonly categories: Category[];
}) {
    return (
        <Table>
            <TableHeader>
                <TableRow>
                    <TableHead>Category</TableHead>
                    <TableHead>Date & Time</TableHead>
                    <TableHead>Note</TableHead>
                    <TableHead>Wallet</TableHead>
                    <TableHead>Amount</TableHead>
                    <TableHead className="text-right pr-5">Actions</TableHead>
                </TableRow>
            </TableHeader>
            <TableBody>
                {transactions.map((transaction) => {
                    const category = categories.find(
                        (category) => category.id === transaction.categoryId
                    );
                    const wallet = wallets.find(
                        (wallet) => wallet.id === transaction.walletId
                    );
                    return (
                        <TableRow key={transaction.id}>
                            <TableCell>
                                <span className="font-medium">
                                    {category?.title ?? "Deleted Category"}
                                </span>
                            </TableCell>
                            <TableCell>
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
                            </TableCell>
                            <TableCell>{transaction?.note ?? "-"}</TableCell>
                            <TableCell>
                                {wallet?.name ?? "Deleted Wallet"}
                            </TableCell>
                            <TableCell className="text-base font-medium">
                                {transaction.transactionType === "Income" ? (
                                    <span className="text-green-700">
                                        {`+${transaction.amount} ${
                                            wallet?.currency ?? "?"
                                        }`}
                                    </span>
                                ) : (
                                    <span className="text-red-600">
                                        {`-${transaction.amount} ${
                                            wallet?.currency ?? "?"
                                        }`}
                                    </span>
                                )}
                            </TableCell>

                            <TableCell className="text-right">
                                <EditCategoryDialog category={categories[0]} />
                                <DeleteCategoryDialog
                                    category={categories[0]}
                                />
                            </TableCell>
                        </TableRow>
                    );
                })}
            </TableBody>
        </Table>
    );
}
