import LoanTabs from "@/components/loans/loan-tabs";

export default function LoansPage() {
    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Track Borrowed and Lent Money
                    </p>
                </div>
                <div className="flex gap-2">Buttons Here</div>
            </div>

            <div className="flex flex-col gap-4">
                <LoanTabs />
            </div>
        </div>
    );
}
