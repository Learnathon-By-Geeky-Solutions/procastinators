import { auth } from "@/lib/auth";
import { Loan } from "@/lib/definitions";

export async function fetchBorrowedLoans() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/loans/borrowed`;
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
            throw new Error("Failed to fetch borrowed loans");
        }
        const data: Loan[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}

export async function fetchLentLoans() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/loans/lent`;
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
            throw new Error("Failed to fetch lent loans");
        }
        const data: Loan[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}
