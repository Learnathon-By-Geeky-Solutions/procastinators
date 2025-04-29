"use client";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useState } from "react";
import BorrowedTab from "@/components/loans/borrowed-tab";
import LentTab from "@/components/loans/lent-tab";
import ClaimableInstallmentsTab from "./claimable-installments-tab";
import ClaimableLoansTab from "./claimable-loans-tab";

const loanFunds = [
    {
        id: "fund1",
        type: "loan",
        amount: 250.0,
        from: "Alex Johnson",
        date: "Apr 15, 2023",
        status: "Pending",
    },
    {
        id: "fund2",
        type: "loan",
        amount: 100.0,
        from: "Lisa Chen",
        date: "Apr 18, 2023",
        status: "Pending",
    },
];

const installmentFunds = [
    {
        id: "fund3",
        type: "installment",
        amount: 50.0,
        from: "Michael Brown",
        date: "Apr 20, 2023",
        status: "Pending",
        loanId: "3",
    },
    {
        id: "fund4",
        type: "installment",
        amount: 75.0,
        from: "David Wilson",
        date: "Apr 22, 2023",
        status: "Pending",
        loanId: "5",
    },
];

export default function ClaimableTabs() {
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

            <ClaimableLoansTab loanFunds={loanFunds} />
            <ClaimableInstallmentsTab installmentFunds={installmentFunds} />
        </Tabs>
    );
}
