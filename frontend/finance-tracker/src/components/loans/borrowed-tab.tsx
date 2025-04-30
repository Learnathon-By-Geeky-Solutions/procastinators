"use client";
import Link from "next/link";
import { Badge } from "@/components/ui/badge";
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
import { TabsContent } from "@/components/ui/tabs";
import { Button } from "@/components/ui/button";
import { Loan } from "@/lib/definitions";
import { toLocaleDateString } from "@/lib/utils";

export default function BorrowedTab({
    loansBorrowed,
}: {
    readonly loansBorrowed: Loan[];
}) {
    return (
        <TabsContent value="borrowed" className="space-y-4">
            <Card>
                <CardHeader>
                    <CardTitle>Money You Borrowed</CardTitle>
                    <CardDescription>
                        Track money you owe to others
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>Status</TableHead>
                                <TableHead>Amount</TableHead>
                                <TableHead>Lender</TableHead>
                                <TableHead>Note</TableHead>
                                <TableHead>Due Amount</TableHead>
                                <TableHead>Due Date</TableHead>
                                <TableHead className="w-[80px]">
                                    Actions
                                </TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {loansBorrowed.length > 0 ? (
                                loansBorrowed.map((loan) => (
                                    <TableRow key={loan.id}>
                                        <TableCell>
                                            {loan.dueAmount > 0 ? (
                                                <Badge
                                                    variant="outline"
                                                    className="bg-yellow-100 text-yellow-800 hover:bg-yellow-100"
                                                >
                                                    Due
                                                </Badge>
                                            ) : (
                                                <Badge
                                                    variant="outline"
                                                    className="bg-green-100 text-green-800 hover:bg-green-100"
                                                >
                                                    Paid
                                                </Badge>
                                            )}
                                        </TableCell>
                                        <TableCell className="text-base font-medium">
                                            {loan?.amount.toFixed(2)} BDT
                                        </TableCell>
                                        <TableCell>
                                            {loan?.lender?.userName ?? "-"}
                                        </TableCell>
                                        <TableCell>
                                            {loan.note ?? "-"}
                                        </TableCell>
                                        <TableCell className=" text-base font-medium">
                                            {loan?.dueAmount.toFixed(2)} BDT
                                        </TableCell>
                                        <TableCell>
                                            {toLocaleDateString(loan.dueDate)}
                                        </TableCell>

                                        <TableCell>
                                            <Link
                                                href={`/dashboard/loans/${loan.id}`}
                                            >
                                                <Button
                                                    variant="outline"
                                                    size="sm"
                                                >
                                                    View
                                                </Button>
                                            </Link>
                                        </TableCell>
                                    </TableRow>
                                ))
                            ) : (
                                <TableRow>
                                    <TableCell
                                        colSpan={8}
                                        className="text-center py-4 text-muted-foreground"
                                    >
                                        No loans found
                                    </TableCell>
                                </TableRow>
                            )}
                        </TableBody>
                    </Table>
                </CardContent>
            </Card>
        </TabsContent>
    );
}
