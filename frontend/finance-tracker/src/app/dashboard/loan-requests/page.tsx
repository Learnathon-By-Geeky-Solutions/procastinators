import LoanRequestTabs from "@/components/loan-requests/loan-request-tabs";

export default async function LoanRequestsPage() {
    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Manage loan requests
                    </p>
                </div>
                <div className="flex gap-2">Buttons Here</div>
            </div>

            <div className="flex flex-col gap-4">
                <LoanRequestTabs />
            </div>
        </div>
    );
}
