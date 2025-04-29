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
import { Eye } from "lucide-react";
import { Button } from "@/components/ui/button";

export default function BorrowedTab({
    loansBorrowed,
}: {
    readonly loansBorrowed: any[];
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
                                <TableHead>Person</TableHead>
                                <TableHead>Amount</TableHead>
                                <TableHead>Remaining</TableHead>
                                <TableHead>Date</TableHead>
                                <TableHead>Due Date</TableHead>
                                <TableHead>Status</TableHead>
                                <TableHead>Note</TableHead>
                                <TableHead className="w-[80px]">
                                    Actions
                                </TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {loansBorrowed.length > 0 ? (
                                loansBorrowed.map((loan) => (
                                    <TableRow key={loan.id}>
                                        <TableCell className="font-medium">
                                            {loan.person}
                                        </TableCell>
                                        <TableCell>
                                            ${loan.amount.toFixed(2)}
                                        </TableCell>
                                        <TableCell>
                                            ${loan.remainingAmount.toFixed(2)}
                                        </TableCell>
                                        <TableCell>{loan.date}</TableCell>
                                        <TableCell>{loan.dueDate}</TableCell>
                                        <TableCell>
                                            <Badge variant="outline">
                                                {loan.status}
                                            </Badge>
                                        </TableCell>
                                        <TableCell>{loan.note}</TableCell>
                                        <TableCell>
                                            <Link href={`/loans/${loan.id}`}>
                                                <Button
                                                    variant="outline"
                                                    size="sm"
                                                >
                                                    <Eye className="h-4 w-4 mr-1" />
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
