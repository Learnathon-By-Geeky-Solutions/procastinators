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
import { InstallmentClaim } from "@/lib/definitions";
import { toLocaleDateString } from "@/lib/utils";
import { HandCoinsIcon } from "lucide-react";
export default function ClaimableInstallmentsTab({
    installmentClaims,
}: {
    readonly installmentClaims: InstallmentClaim[];
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
                                <TableHead>Amount</TableHead>
                                <TableHead>From</TableHead>
                                <TableHead>Date</TableHead>
                                <TableHead>Status</TableHead>
                                <TableHead className="w-[100px]">
                                    Actions
                                </TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {installmentClaims.length > 0 ? (
                                installmentClaims.map((claim) => (
                                    <TableRow key={claim.id}>
                                        <TableCell>
                                            {claim?.installment?.amount.toFixed(
                                                2
                                            )}{" "}
                                            BDT
                                        </TableCell>
                                        <TableCell className="font-medium">
                                            {
                                                claim?.installment.loan
                                                    ?.borrower?.userName
                                            }
                                        </TableCell>
                                        <TableCell>
                                            {toLocaleDateString(
                                                claim?.installment?.timestamp
                                            )}
                                        </TableCell>
                                        <TableCell>
                                            {claim.isClaimed ? (
                                                <Badge
                                                    variant="outline"
                                                    className="bg-green-100 text-green-800 hover:bg-green-100"
                                                >
                                                    Claimed
                                                </Badge>
                                            ) : (
                                                <Badge
                                                    variant="outline"
                                                    className="bg-yellow-100 text-yellow-800 hover:bg-yellow-100"
                                                >
                                                    Pending
                                                </Badge>
                                            )}
                                        </TableCell>
                                        <TableCell>
                                            <Button variant="outline" size="sm">
                                                <HandCoinsIcon className="h-4 w-4" />
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
