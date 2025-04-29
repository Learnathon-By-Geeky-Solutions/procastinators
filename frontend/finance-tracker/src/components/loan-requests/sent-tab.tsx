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
                                    <TableHead>Recipient</TableHead>
                                    <TableHead>Amount</TableHead>
                                    <TableHead>Date</TableHead>
                                    <TableHead>Due Date</TableHead>
                                    <TableHead>Note</TableHead>
                                    <TableHead>Status</TableHead>
                                    <TableHead className="w-[100px]">
                                        Actions
                                    </TableHead>
                                </TableRow>
                            </TableHeader>
                            <TableBody>
                                {sentRequests.map((request) => (
                                    <TableRow key={request.id}>
                                        <TableCell className="font-medium">
                                            {request.recipient}
                                        </TableCell>
                                        <TableCell>
                                            ${request?.amount.toFixed(2)}
                                        </TableCell>
                                        <TableCell>{request.date}</TableCell>
                                        <TableCell>{request.dueDate}</TableCell>
                                        <TableCell>{request.note}</TableCell>
                                        <TableCell>
                                            <Badge
                                                variant={
                                                    request.status ===
                                                    "Approved"
                                                        ? "default"
                                                        : request.status ===
                                                          "Rejected"
                                                        ? "destructive"
                                                        : "outline"
                                                }
                                            >
                                                {request.status}
                                            </Badge>
                                        </TableCell>
                                        <TableCell>
                                            {request.status === "Pending" && (
                                                <Button
                                                    variant="outline"
                                                    size="sm"
                                                >
                                                    Cancel
                                                </Button>
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
