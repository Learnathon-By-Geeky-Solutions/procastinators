import LoanTabs from "@/components/loans/loan-tabs";
import { fetchBorrowedLoans, fetchLentLoans } from "@/lib/data/loan-data";

export default async function LoansPage() {
    const loansBorrowed = await fetchBorrowedLoans();
    const loansLent = await fetchLentLoans();

    console.log("loansBorrowed", loansBorrowed);
    console.log("loansLent", loansLent);

    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Track borrowed and lent money
                    </p>
                </div>
                <div className="flex gap-2">Buttons Here</div>
            </div>

            <div className="flex flex-col gap-4">
                <LoanTabs loansBorrowed={loansBorrowed} loansLent={loansLent} />
            </div>
        </div>
    );
}
