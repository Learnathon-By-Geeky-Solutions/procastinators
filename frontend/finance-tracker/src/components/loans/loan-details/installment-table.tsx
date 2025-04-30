import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { Installment } from "@/lib/definitions";
import { toLocaleDateString } from "@/lib/utils";

export default function InstallmentTable({
    installments,
}: {
    readonly installments: Installment[];
}) {
    return (
        <Card>
            <CardHeader>
                <CardTitle>Installments</CardTitle>
                <CardDescription>
                    History of payments for this loan
                </CardDescription>
            </CardHeader>
            <CardContent>
                {installments.length > 0 ? (
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>Amount</TableHead>
                                <TableHead>Date</TableHead>
                                <TableHead>Note</TableHead>
                                <TableHead>Next Due Date</TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {installments.map((installment) => (
                                <TableRow key={installment.id}>
                                    <TableCell className="tex-base font-medium">
                                        {installment.amount.toFixed(2)} BDT
                                    </TableCell>
                                    <TableCell>
                                        {toLocaleDateString(
                                            installment.timestamp
                                        )}
                                    </TableCell>
                                    <TableCell>
                                        {installment.note ?? "-"}
                                    </TableCell>
                                    <TableCell>
                                        {toLocaleDateString(
                                            installment.nextDueDate
                                        )}
                                    </TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                ) : (
                    <div className="text-center py-8 text-muted-foreground">
                        <p>No installments have been made yet.</p>
                    </div>
                )}
            </CardContent>
        </Card>
    );
}
