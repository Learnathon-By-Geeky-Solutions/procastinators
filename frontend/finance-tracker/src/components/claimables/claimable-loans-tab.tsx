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
import { LoanClaim } from "@/lib/definitions";
export default function ClaimableLoansTab({
    loanClaims,
}: {
    readonly loanClaims: LoanClaim[];
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
                            {loanClaims.length > 0 ? (
                                loanClaims.map((claim) => (
                                    <TableRow key={claim.id}>
                                        <TableCell className="font-medium">
                                            {claim?.loan?.lender?.userName}
                                        </TableCell>
                                        <TableCell>
                                            ${claim?.loan?.amount.toFixed(2)}
                                        </TableCell>
                                        <TableCell>
                                            {claim?.loan?.issuedAt}
                                        </TableCell>
                                        <TableCell>
                                            {claim?.isClaimed ? (
                                                <Badge variant="outline">
                                                    Claimed
                                                </Badge>
                                            ) : (
                                                <Badge variant="default">
                                                    Unclaimed
                                                </Badge>
                                            )}
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
