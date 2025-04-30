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
import { toLocaleDateString } from "@/lib/utils";

export default function SentTab({
    sentRequests,
}: {
    readonly sentRequests: any[];
}) {
    return (
        <TabsContent value="sent" className="space-y-4">
            <Card>
                <CardHeader>
                    <CardTitle>Sent Loan Requests</CardTitle>
                    <CardDescription>
                        Requests you've sent to borrow money from others
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    {sentRequests.length > 0 ? (
                        <Table>
                            <TableHeader>
                                <TableRow>
                                    <TableHead>Status</TableHead>
                                    <TableHead>Recipient</TableHead>
                                    <TableHead>Amount</TableHead>
                                    <TableHead>Note</TableHead>
                                    <TableHead>Due Date</TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {sentRequests.map((request) => (
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
                                            {request?.lender?.userName}
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
                                    </TableRow>
                                ))}
                            </TableBody>
                        </Table>
                    ) : (
                        <div className="text-center py-8 text-muted-foreground">
                            <p>You haven't sent any loan requests.</p>
                        </div>
                    )}
                </CardContent>
            </Card>
        </TabsContent>
    );
}
