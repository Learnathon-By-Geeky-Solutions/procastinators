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
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import { InstallmentClaim, Wallet } from "@/lib/definitions";
import { toLocaleDateString } from "@/lib/utils";
import { ClaimInstallmentDialog } from "@/components/claimables/claim-installment-dialog";
export default function ClaimableInstallmentsTab({
    installmentClaims,
    wallets,
}: {
    readonly installmentClaims: InstallmentClaim[];
    readonly wallets: Wallet[];
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
                                <TableHead>Status</TableHead>
                                <TableHead>Amount</TableHead>
                                <TableHead>From</TableHead>
                                <TableHead>Date</TableHead>
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
                                            {!claim?.isClaimed && (
                                                <ClaimInstallmentDialog
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
                                        No installments found
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
