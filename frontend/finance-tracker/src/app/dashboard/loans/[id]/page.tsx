import Link from "next/link";
import { Button } from "@/components/ui/button";
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";

import { ArrowLeft, ArrowDownLeft, ArrowUpRight } from "lucide-react";
import LoanInfoCard from "@/components/loans/loan-details/loan-info-card";
import { fetchLoanById } from "@/lib/data/loan-data";
import LoanSummaryCard from "@/components/loans/loan-details/loan-summary-card";
import { fetchInstallmentsByLoanId } from "@/lib/data/installments-data";
import InstallmentTable from "@/components/loans/loan-details/installment-table";

export default async function LoanDetailPage({
    params,
}: {
    params: { id: string };
}) {
    // Find the loan by ID
    const loan = await fetchLoanById(params.id);

    if (!loan) {
        return (
            <div className="flex flex-col gap-4 p-4 md:p-8">
                <div className="flex items-center gap-2">
                    <Link href="/dashboard/loans">
                        <Button variant="outline" size="sm">
                            <ArrowLeft className="h-4 w-4 mr-2" />
                            Back to Loans
                        </Button>
                    </Link>
                </div>
                <Card>
                    <CardContent className="flex flex-col items-center justify-center py-12">
                        <h2 className="text-xl font-semibold mb-2">
                            Loan Not Found
                        </h2>
                        <p className="text-muted-foreground">
                            The loan you're looking for doesn't exist or you
                            don't have permission to view it.
                        </p>
                    </CardContent>
                </Card>
            </div>
        );
    }

    const installments = await fetchInstallmentsByLoanId(params.id);

    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center gap-2">
                <Link href="/dashboard/loans">
                    <Button variant="outline" size="sm">
                        <ArrowLeft className="h-4 w-4 mr-2" />
                        Back to Loans
                    </Button>
                </Link>
            </div>

            <div className="flex items-center justify-between mb-2">
                <div>
                    <h1 className="text-2xl font-bold tracking-tight">
                        Loan Details
                    </h1>
                </div>
                <div>Buttons Here</div>
            </div>

            <div className="grid gap-4 lg:grid-cols-2">
                <LoanInfoCard loan={loan} />
                <LoanSummaryCard loan={loan} />
            </div>

            <InstallmentTable installments={installments} />
        </div>
    );
}
