"use client";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useState } from "react";
import ReceivedTab from "./received-tab";
import SentTab from "./sent-tab";
import { LoanRequest, Wallet } from "@/lib/definitions";

export default function LoanRequestTabs({
    receivedRequests,
    sentRequests,
    wallets,
}: {
    receivedRequests: LoanRequest[];
    sentRequests: LoanRequest[];
    wallets: Wallet[];
}) {
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

            <ReceivedTab
                receivedRequests={receivedRequests}
                wallets={wallets}
            />
            <SentTab sentRequests={sentRequests} />
        </Tabs>
    );
}
