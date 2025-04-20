import { auth } from "@/lib/auth";
import { Transaction, TransactionReport } from "@/lib/definitions";

export async function fetchTransactions() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/personaltransactions`;
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
            throw new Error("Failed to fetch categories");
        }

        const data: Transaction[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}

export async function fetchTransactionReport(type?: string, days?: number) {
    try {
        const queryParams = [
            type ? `type=${type}` : null,
            days ? `days=${days}` : null,
        ];
        const query = queryParams.filter(Boolean).join("&");
        const url = `${process.env.BACKEND_BASE_URL}/personaltransactions/report/categories?${query}`;
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
            throw new Error("Failed to fetch categories");
        }
        const data: TransactionReport = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}
