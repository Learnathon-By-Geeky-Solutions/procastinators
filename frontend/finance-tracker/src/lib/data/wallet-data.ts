import { auth } from "@/lib/auth";
import { Wallet } from "@/lib/definitions";

export async function fetchWallets() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/wallets`;
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
            throw new Error("Failed to fetch wallets");
        }

        const data: Wallet[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}

export async function fetchTotalBalance() {
    const wallets = await fetchWallets();
    return wallets.reduce((acc, wallet) => acc + wallet.balance, 0);
}
