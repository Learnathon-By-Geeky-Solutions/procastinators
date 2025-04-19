import { auth } from "@/lib/auth";
import { Category } from "@/lib/definitions";

export async function fetchCategories() {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/categories`;
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

        const data: Category[] = await res.json();
        return data;
    } catch (error) {
        console.error(error);
        throw error;
    }
}
