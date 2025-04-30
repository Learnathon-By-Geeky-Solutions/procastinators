"use client";
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
import { LoanRequest, Wallet } from "@/lib/definitions";
import { toLocaleDateString } from "@/lib/utils";
import { ApproveLoanRequestDialog } from "@/components/loan-requests/approve-loan-request-dialog";

export default function ReceivedTab({
    receivedRequests,
    wallets,
}: {
    readonly receivedRequests: LoanRequest[];
    readonly wallets: Wallet[];
}) {
    return (
        <TabsContent value="received" className="space-y-4">
            <Card>
                <CardHeader>
                    <CardTitle>Received Loan Requests</CardTitle>
                    <CardDescription>
                        Requests from other users asking to borrow money from
                        you
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    {receivedRequests.length > 0 ? (
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Status</TableHead>
                                    <TableHead>Requester</TableHead>
                                    <TableHead>Amount</TableHead>
                                    <TableHead>Note</TableHead>
                                    <TableHead>Due Date</TableHead>
                                    <TableHead className="w-[80px]">
                                        Actions
                                    </TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {receivedRequests.map((request) => (
                                    <TableRow key={request.id}>
                                        <TableCell>
                                            {request.isApproved ? (
                                                <Badge
                                                    variant="outline"
                                                    className="bg-green-100 text-green-800 hover:bg-green-100"
                                                >
                                                    Accepted
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
                                            {request?.borrower?.userName}
                                        </TableCell>
                                        <TableCell className="text-base font-medium">
                                            {request?.amount.toFixed(2)} BDT
                                        </TableCell>
                                        <TableCell>{request.note}</TableCell>
                                        <TableCell>
                                            {toLocaleDateString(
                                                request.dueDate
                                            )}
                                        </TableCell>

                                        <TableCell>
                                            {!request.isApproved && (
                                                <ApproveLoanRequestDialog
                                                    request={request}
                                                    wallets={wallets}
                                                />
                                            )}
                                        </TableCell>
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    ) : (
                        <div className="text-center py-8 text-muted-foreground">
                            <p>You don't have any received loan requests.</p>
                        </div>
                    )}
                </CardContent>
            </Card>
        </TabsContent>
    );
}
