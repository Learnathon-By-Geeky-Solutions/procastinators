"use client";
import { Badge } from "@/components/ui/badge";
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
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
export default function ClaimableInstallmentsTab({
    installmentFunds,
}: {
    readonly installmentFunds: any[];
}) {
    return (
        <TabsContent value="installments" className="space-y-4">
            <Card>
                <CardHeader>
                    <CardTitle>Loan Funds</CardTitle>
                    <CardDescription>
                        Claim received installments
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>From</TableHead>
                                <TableHead>Amount</TableHead>
                                <TableHead>Date</TableHead>
                                <TableHead>Status</TableHead>
                                <TableHead className="w-[100px]">
                                    Actions
                                </TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {installmentFunds.length > 0 ? (
                                installmentFunds.map((fund) => (
                                    <TableRow key={fund.id}>
                                        <TableCell className="font-medium">
                                            {fund.from}
                                        </TableCell>
                                        <TableCell>
                                            ${fund.amount.toFixed(2)}
                                        </TableCell>
                                        <TableCell>{fund.date}</TableCell>
                                        <TableCell>
                                            <Badge variant="outline">
                                                {fund.status}
                                            </Badge>
                                        </TableCell>
                                        <TableCell>
                                            <Button variant="outline" size="sm">
                                                Claim
                                            </Button>
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
