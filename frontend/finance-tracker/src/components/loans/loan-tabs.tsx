"use client";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useState } from "react";
import BorrowedTab from "@/components/loans/borrowed-tab";
import LentTab from "@/components/loans/lent-tab";

// Mock data for loans
const loansBorrowed = [
    {
        id: "1",
        person: "John Smith",
        amount: 500.0,
        remainingAmount: 500.0,
        date: "Mar 15, 2023",
        dueDate: "May 15, 2023",
        status: "Active",
        note: "Emergency fund",
        loanRequestId: null,
    },
    {
        id: "2",
        person: "Sarah Johnson",
        amount: 200.0,
        remainingAmount: 150.0,
        date: "Apr 2, 2023",
        dueDate: "Jun 2, 2023",
        status: "Active",
        note: "Car repair",
        loanRequestId: "req2",
    },
];

const loansLent = [
    {
        id: "3",
        person: "Michael Brown",
        amount: 150.0,
        remainingAmount: 150.0,
        date: "Feb 20, 2023",
        dueDate: "Apr 20, 2023",
        status: "Overdue",
        note: "Phone bill",
        loanRequestId: null,
    },
    {
        id: "4",
        person: "Emily Davis",
        amount: 300.0,
        remainingAmount: 300.0,
        date: "Mar 10, 2023",
        dueDate: "May 10, 2023",
        status: "Active",
        note: "Laptop purchase",
        loanRequestId: "req4",
    },
    {
        id: "5",
        person: "David Wilson",
        amount: 75.0,
        remainingAmount: 75.0,
        date: "Apr 5, 2023",
        dueDate: "May 5, 2023",
        status: "Active",
        note: "Dinner",
        loanRequestId: "req5",
    },
];

export default function LoanTabs() {
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
