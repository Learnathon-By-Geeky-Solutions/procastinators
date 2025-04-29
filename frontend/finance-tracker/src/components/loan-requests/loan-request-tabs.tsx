"use client";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useState } from "react";
import ReceivedTab from "./received-tab";
import SentTab from "./sent-tab";

const receivedRequests = [
    {
        id: "req1",
        requester: "Alex Johnson",
        amount: 250.0,
        date: "Apr 10, 2023",
        dueDate: "May 10, 2023",
        status: "Pending",
        note: "Need to pay rent",
    },
    {
        id: "req2",
        requester: "Lisa Chen",
        amount: 100.0,
        date: "Apr 12, 2023",
        dueDate: "May 12, 2023",
        status: "Pending",
        note: "Grocery shopping",
    },
];

const sentRequests = [
    {
        id: "req3",
        recipient: "James Wilson",
        amount: 150.0,
        date: "Apr 8, 2023",
        dueDate: "May 8, 2023",
        status: "Pending",
        note: "Car repair",
    },
    {
        id: "req4",
        recipient: "Emma Thompson",
        amount: 75.0,
        date: "Apr 5, 2023",
        dueDate: "May 5, 2023",
        status: "Approved",
        note: "Phone bill",
    },
    {
        id: "req5",
        recipient: "Robert Garcia",
        amount: 200.0,
        date: "Apr 3, 2023",
        dueDate: "May 3, 2023",
        status: "Rejected",
        note: "Laptop repair",
    },
];

export default function LoanRequestTabs() {
    const [activeTab, setActiveTab] = useState("received");

    return (
        <Tabs
            defaultValue="received"
            value={activeTab}
            onValueChange={setActiveTab}
            className="space-y-4"
        >
            <TabsList>
                <TabsTrigger className="w-24 p-2" value="received">
                    Received
                </TabsTrigger>
                <TabsTrigger className="w-24 p-2" value="sent">
                    Sent
                </TabsTrigger>
            </TabsList>

            <ReceivedTab receivedRequests={receivedRequests} />
            <SentTab sentRequests={sentRequests} />
        </Tabs>
    );
}
