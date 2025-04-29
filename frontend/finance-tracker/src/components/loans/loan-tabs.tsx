"use client";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useState } from "react";
import BorrowedTab from "@/components/loans/borrowed-tab";
import LentTab from "@/components/loans/lent-tab";

export default function LoanTabs({
    loansBorrowed,
    loansLent,
}: {
    loansBorrowed: any[];
    loansLent: any[];
}) {
    const [activeTab, setActiveTab] = useState("borrowed");

    return (
        <Tabs
            defaultValue="borrowed"
            value={activeTab}
            onValueChange={setActiveTab}
            className="space-y-4"
        >
            <TabsList>
                <TabsTrigger className="w-24 p-2" value="borrowed">
                    Borrowed
                </TabsTrigger>
                <TabsTrigger className="w-24 p-2" value="lent">
                    Lent
                </TabsTrigger>
            </TabsList>

            <BorrowedTab loansBorrowed={loansBorrowed} />
            <LentTab loansLent={loansLent} />
        </Tabs>
    );
}
