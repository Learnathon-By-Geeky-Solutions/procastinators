import LoanRequestTabs from "@/components/loan-requests/loan-request-tabs";
import { RequestLoanDialog } from "@/components/loans/request-loan-dialog";
import {
    fetchReceivedLoanRequests,
    fetchSentLoanRequests,
} from "@/lib/data/loan-request-data";
import { fetchWallets } from "@/lib/data/wallet-data";

export default async function LoanRequestsPage() {
    const receivedRequests = await fetchReceivedLoanRequests();
    const sentRequests = await fetchSentLoanRequests();
    const wallets = await fetchWallets();
    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Manage loan requests
                    </p>
                </div>
                <div className="flex gap-2">
                    <RequestLoanDialog variant="default" />
                </div>
            </div>

            <div className="flex flex-col gap-4">
                <LoanRequestTabs
                    receivedRequests={receivedRequests}
                    wallets={wallets}
                    sentRequests={sentRequests}
                />
            </div>
        </div>
    );
}
