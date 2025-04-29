"use client";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useState } from "react";
import BorrowedTab from "@/components/loans/borrowed-tab";
import LentTab from "@/components/loans/lent-tab";
import ClaimableInstallmentsTab from "./claimable-installments-tab";
import ClaimableLoansTab from "./claimable-loans-tab";
import { InstallmentClaim, LoanClaim } from "@/lib/definitions";

export default function ClaimableTabs({
    loanClaims,
    installmentClaims,
}: {
    readonly loanClaims: LoanClaim[];
    readonly installmentClaims: InstallmentClaim[];
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

            <ClaimableLoansTab loanClaims={loanClaims} />
            <ClaimableInstallmentsTab installmentClaims={installmentClaims} />
        </Tabs>
    );
}
