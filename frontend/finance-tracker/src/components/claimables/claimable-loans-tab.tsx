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
import { LoanClaim, Wallet } from "@/lib/definitions";
import { toLocaleDateString } from "@/lib/utils";
import { ClaimLoanDialog } from "./claim-loan-dialog";
export default function ClaimableLoansTab({
    loanClaims,
    wallets,
}: {
    readonly loanClaims: LoanClaim[];
    readonly wallets: Wallet[];
}) {
    return (
        <TabsContent value="loans" className="space-y-4">
            <Card>
                <CardHeader>
                    <CardTitle>Loan Funds</CardTitle>
                    <CardDescription>Claim approved loans</CardDescription>
                </CardHeader>
                <CardContent>
                    <Table>
                        <TableHeader>
                            <TableRow>
                                <TableHead>Status</TableHead>
                                <TableHead>Amount</TableHead>
                                <TableHead>From</TableHead>
                                <TableHead>Issued At</TableHead>
                                <TableHead>Note</TableHead>
                                <TableHead className="w-[100px]">
                                    Actions
                                </TableHead>
                            </TableRow>
                        </TableHeader>
                        <TableBody>
                            {loanClaims.length > 0 ? (
                                loanClaims.map((claim) => (
                                    <TableRow key={claim.id}>
                                        <TableCell>
                                            {claim?.isClaimed ? (
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
                                        <TableCell className="text-base font-medium">
                                            {claim?.loan?.amount.toFixed(2)} BDT
                                        </TableCell>
                                        <TableCell className="font-medium">
                                            {claim?.loan?.lender?.userName}
                                        </TableCell>
                                        <TableCell>
                                            {toLocaleDateString(
                                                claim?.loan?.issuedAt
                                            )}
                                        </TableCell>
                                        <TableCell>
                                            {claim?.loan?.note ?? "-"}
                                        </TableCell>

                                        <TableCell>
                                            {!claim?.isClaimed && (
                                                <ClaimLoanDialog
                                                    claim={claim}
                                                    wallets={wallets}
                                                />
                                            )}
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
