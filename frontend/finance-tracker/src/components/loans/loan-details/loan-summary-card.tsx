import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Progress } from "@/components/ui/progress";
import { Loan } from "@/lib/definitions";

export default function LoanSummaryCard({ loan }: { loan: Loan }) {
    const paidAmount = loan.amount - loan.dueAmount;
    const paymentProgress = Math.round((paidAmount / loan.amount) * 100);
    return (
        <Card>
            <CardHeader>
                <div className="flex items-center justify-between">
                    <CardTitle>Payment Summary</CardTitle>
                    <div>
                        {loan.dueAmount > 0 ? (
                            <Badge
                                variant="outline"
                                className="bg-yellow-100 text-yellow-800 hover:bg-yellow-100"
                            >
                                Due
                            </Badge>
                        ) : (
                            <Badge
                                variant="outline"
                                className="bg-green-100 text-green-800 hover:bg-green-100"
                            >
                                Paid
                            </Badge>
                        )}
                    </div>
                </div>
            </CardHeader>
            <CardContent>
                <div className="space-y-4">
                    <div className="flex items-center justify-between">
                        <span className="text-sm font-medium text-muted-foreground">
                            Total Amount
                        </span>
                        <span className="font-semibold">
                            {loan.amount.toFixed(2)} BDT
                        </span>
                    </div>
                    <div className="flex items-center justify-between">
                        <span className="text-sm font-medium text-muted-foreground">
                            Paid Amount
                        </span>
                        <span className="font-semibold">
                            {paidAmount.toFixed(2)} BDT
                        </span>
                    </div>
                    <div className="flex items-center justify-between">
                        <span className="text-sm font-medium text-muted-foreground">
                            Due Amount
                        </span>
                        <span className="font-semibold">
                            {loan.dueAmount.toFixed(2)} BDT
                        </span>
                    </div>
                    <div className="pt-4 border-t">
                        <div className="flex items-center justify-between">
                            <span className="text-sm font-medium">
                                Payment Progress
                            </span>
                            <span className="text-sm font-medium">
                                {paymentProgress}%
                            </span>
                        </div>
                        <div className="w-full h-2 bg-muted rounded-full mt-2 overflow-hidden">
                            <Progress
                                className="border-1"
                                value={paymentProgress}
                            />
                        </div>
                    </div>
                </div>
            </CardContent>
        </Card>
    );
}
