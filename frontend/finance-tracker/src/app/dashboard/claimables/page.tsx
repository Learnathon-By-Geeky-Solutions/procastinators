import ClaimableTabs from "@/components/claimables/claimable-tabs";
import {
    fetchInstallmentClaims,
    fetchLoanClaims,
} from "@/lib/data/claimables-data";

export default async function ClaimablesPage() {
    const loanClaims = await fetchLoanClaims();
    const installmentClaims = await fetchInstallmentClaims();
    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Claim your approved loans and received installments
                    </p>
                </div>
                <div className="flex gap-2">Buttons Here</div>
            </div>

            <div className="flex flex-col gap-4">
                <ClaimableTabs
                    loanClaims={loanClaims}
                    installmentClaims={installmentClaims}
                />
            </div>
        </div>
    );
}
