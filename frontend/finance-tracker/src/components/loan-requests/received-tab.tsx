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

export default function ReceivedTab({
    receivedRequests,
}: {
    readonly receivedRequests: any[];
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
                                    <TableHead>Date</TableHead>
                                    <TableHead>Due Date</TableHead>
                                    <TableHead>Note</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead className="w-[150px]">
                                        Actions
                                    </TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {receivedRequests.map((request) => (
                                    <TableRow key={request.id}>
                                        <TableCell className="font-medium">
                                            {request.requester}
                                        </TableCell>
                                        <TableCell>
                                            ${request.amount.toFixed(2)}
                                        </TableCell>
                                        <TableCell>{request.date}</TableCell>
                                        <TableCell>{request.dueDate}</TableCell>
                                        <TableCell>{request.note}</TableCell>
                                        <TableCell>
                                            <Badge variant="outline">
                                                {request.status}
                                            </Badge>
                                        </TableCell>
                                        <TableCell>
                                            <div className="flex gap-2">
                                                <Button
                                                    variant="outline"
                                                    size="sm"
                                                    className="bg-emerald-50 hover:bg-emerald-100 border-emerald-200"
                                                >
                                                    <Check className="h-4 w-4 mr-1" />
                                                    Approve
                                                </Button>
                                            </div>
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
