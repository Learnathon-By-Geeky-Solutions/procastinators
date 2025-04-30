import { AddLoanDialog } from "@/components/loans/add-loan-dialog";
import LoanTabs from "@/components/loans/loan-tabs";
import { fetchBorrowedLoans, fetchLentLoans } from "@/lib/data/loan-data";
import { fetchWallets } from "@/lib/data/wallet-data";

export default async function LoansPage() {
    const loansBorrowed = await fetchBorrowedLoans();
    const loansLent = await fetchLentLoans();
    const wallets = await fetchWallets();

    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Track borrowed and lent money
                    </p>
                </div>
                <div className="flex gap-2">
                    <AddLoanDialog wallets={wallets} />
                </div>
            </div>

            <div className="flex flex-col gap-4">
                <LoanTabs loansBorrowed={loansBorrowed} loansLent={loansLent} />
            </div>
        </div>
    );
}
