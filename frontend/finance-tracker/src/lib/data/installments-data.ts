import { auth } from "@/lib/auth";
import { Installment } from "@/lib/definitions";

export async function fetchInstallmentsByLoanId(loanId: string) {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/loans/${loanId}/installments`;
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
            throw new Error("Failed to fetch installments");
        }
        const data: Installment[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}
