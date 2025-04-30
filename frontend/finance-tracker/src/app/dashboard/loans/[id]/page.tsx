import Link from "next/link";
import { Button } from "@/components/ui/button";
import { Card, CardContent } from "@/components/ui/card";

import { ArrowLeft } from "lucide-react";
import LoanInfoCard from "@/components/loans/loan-details/loan-info-card";
import { fetchLoanById } from "@/lib/data/loan-data";
import LoanSummaryCard from "@/components/loans/loan-details/loan-summary-card";
import { fetchInstallmentsByLoanId } from "@/lib/data/installments-data";
import InstallmentTable from "@/components/loans/loan-details/installment-table";
import { PayInsallmentDialog } from "@/components/loans/loan-details/pay-installment-dialog";
import { auth } from "@/lib/auth";
import { fetchWallets } from "@/lib/data/wallet-data";

export default async function LoanDetailPage({
    params,
}: {
    params: Promise<{ id: string }>;
}) {
    const { id } = await params;
    const loan = await fetchLoanById(id);

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

    const installments = await fetchInstallmentsByLoanId(id);
    const wallets = await fetchWallets();
    const session = await auth();
    const email = session?.user?.email;
    const action = loan.borrower?.email === email ? "pay" : "receive";

    const isAllowed =
        (action === "pay" && loan.dueAmount > 0) ||
        (action === "receive" && loan.dueAmount > 0 && loan.borrower == null);

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
                <div>
                    {isAllowed && (
                        <PayInsallmentDialog
                            action={action}
                            wallets={wallets}
                            loan={loan}
                        />
                    )}
                </div>
            </div>

            <div className="grid gap-4 lg:grid-cols-2">
                <LoanInfoCard loan={loan} />
                <LoanSummaryCard loan={loan} />
            </div>

            <InstallmentTable installments={installments} />
        </div>
    );
}
