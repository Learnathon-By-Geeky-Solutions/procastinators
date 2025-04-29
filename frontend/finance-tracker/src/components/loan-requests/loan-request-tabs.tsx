"use client";
import { Tabs, TabsList, TabsTrigger } from "@/components/ui/tabs";
import { useState } from "react";
import ReceivedTab from "./received-tab";
import SentTab from "./sent-tab";

export default function LoanRequestTabs({
    receivedRequests,
    sentRequests,
}: {
    receivedRequests: any[];
    sentRequests: any[];
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

            <ReceivedTab receivedRequests={receivedRequests} />
            <SentTab sentRequests={sentRequests} />
        </Tabs>
    );
}
