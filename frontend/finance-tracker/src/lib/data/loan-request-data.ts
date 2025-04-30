import { LoanRequest } from "@/lib/definitions";
import { auth } from "@/lib/auth";

export async function fetchReceivedLoanRequests() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/loanrequests/received`;
        const session = await auth();
        const token = session?.accessToken;
        const res = await fetch(url, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            },
        });
        if (!res.ok) {
            throw new Error("Failed to fetch received loan requests");
        }
        const data: LoanRequest[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}

export async function fetchSentLoanRequests() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/loanrequests/sent`;
        const session = await auth();
        const token = session?.accessToken;
        const res = await fetch(url, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
                "Content-Type": "application/json",
            },
        });
        if (!res.ok) {
            throw new Error("Failed to fetch sent loan requests");
        }
        const data: LoanRequest[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}
