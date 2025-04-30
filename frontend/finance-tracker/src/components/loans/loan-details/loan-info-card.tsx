import { Badge } from "@/components/ui/badge";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Loan } from "@/lib/definitions";
import { toLocaleDateString } from "@/lib/utils";

export default function LoanInfoCard({ loan }: { loan: Loan }) {
    return (
        <Card>
            <CardHeader>
                <CardTitle>Loan Information</CardTitle>
            </CardHeader>
            <CardContent>
                <dl className="grid grid-cols-1 gap-4 sm:grid-cols-2">
                    <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                            Borrower
                        </dt>
                        <dd className="text-sm font-semibold">
                            {loan?.borrower?.userName ?? "-"}
                        </dd>
                    </div>
                    <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                            Lender
                        </dt>
                        <dd className="text-sm font-semibold">
                            {loan?.lender?.userName ?? "-"}
                        </dd>
                    </div>

                    <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                            Date
                        </dt>
                        <dd className="text-sm">
                            {toLocaleDateString(loan?.issuedAt)}
                        </dd>
                    </div>
                    <div>
                        <dt className="text-sm font-medium text-muted-foreground">
                            Due Date
                        </dt>
                        <dd className="text-sm">
                            {toLocaleDateString(loan?.dueDate)}
                        </dd>
                    </div>
                    <div className="sm:col-span-2">
                        <dt className="text-sm font-medium text-muted-foreground">
                            Note
                        </dt>
                        <dd className="text-sm">
                            {loan.note ?? "No additional note"}
                        </dd>
                    </div>
                </dl>
            </CardContent>
        </Card>
    );
}
