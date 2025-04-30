import { auth } from "@/lib/auth";
import { InstallmentClaim, LoanClaim } from "../definitions";

export async function fetchLoanClaims() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/loanclaims`;
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
            throw new Error("Failed to fetch claimable loans");
        }
        const data: LoanClaim[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}

export async function fetchInstallmentClaims() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/installmentclaims`;
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
            throw new Error("Failed to fetch claimable installments");
        }
        const data: InstallmentClaim[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}
