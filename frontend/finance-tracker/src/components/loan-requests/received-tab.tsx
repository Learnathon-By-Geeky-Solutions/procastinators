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
import { Check, Eye } from "lucide-react";
import { Button } from "@/components/ui/button";
import { LoanRequest } from "@/lib/definitions";
import { toLocaleDateString } from "@/lib/utils";

export default function ReceivedTab({
    receivedRequests,
}: {
    readonly receivedRequests: LoanRequest[];
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
                                    <TableHead>Requester</TableHead>
                                    <TableHead>Amount</TableHead>
                                    <TableHead>Note</TableHead>
                                    <TableHead>Due Date</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead className="w-[80px]">
                                        Actions
                                    </TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {receivedRequests.map((request) => (
                                    <TableRow key={request.id}>
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
                                            {!request.isApproved && (
                                                <Button
                                                    // variant="outline"
                                                    size="sm"
                                                >
                                                    <Check className="h-4 w-4" />
                                                    Accept
                                                </Button>
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
