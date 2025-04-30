"use client";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useState } from "react";
import ClaimableInstallmentsTab from "./claimable-installments-tab";
import ClaimableLoansTab from "./claimable-loans-tab";
import { InstallmentClaim, LoanClaim, Wallet } from "@/lib/definitions";

export default function ClaimableTabs({
    loanClaims,
    installmentClaims,
    wallets,
}: {
    readonly loanClaims: LoanClaim[];
    readonly installmentClaims: InstallmentClaim[];
    readonly wallets: Wallet[];
}) {
    const [activeTab, setActiveTab] = useState("loans");

    return (
        <Tabs
            defaultValue="loans"
            value={activeTab}
            onValueChange={setActiveTab}
            className="space-y-4"
        >
            <TabsList>
                <TabsTrigger className="w-24 p-2" value="loans">
                    Loan
                </TabsTrigger>
                <TabsTrigger className="w-24 p-2" value="installments">
                    Installment
                </TabsTrigger>
            </TabsList>

            <ClaimableLoansTab loanClaims={loanClaims} wallets={wallets} />
            <ClaimableInstallmentsTab installmentClaims={installmentClaims} />
        </Tabs>
    );
}
