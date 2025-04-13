"use server";
import { auth } from "@/lib/auth";
import { revalidatePath } from "next/cache";

const defaultErrorMessage = "Something went wrong";

function MapFieldErrors(errors: Record<string, string[]>) {
    return Object.entries(errors ?? {}).reduce((obj, [key, value]) => {
        obj[key.toLocaleLowerCase()] = Array.isArray(value) ? value : [value];
        return obj;
    }, {} as Record<string, string[]>);
}

export async function handleAction(httpMethod: string, url: string, body: any) {
    try {
        const session = await auth();
        const res = await fetch(url, {
            method: httpMethod,
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${session?.accessToken}`,
            },
            body: JSON.stringify(body),
        });

        if (!res.ok) {
            const data = await res.json();
            console.log(data);

            return {
                success: false,
                fieldErrors: { ...MapFieldErrors(data.errors) },
                message: data?.title ?? defaultErrorMessage,
            };
        }

        revalidatePath("/");
        return {
            success: true,
            fieldErrors: {},
            message: "",
        };
    } catch (error) {
        console.log(error);
        return {
            success: false,
            fieldErrors: {},
            message: defaultErrorMessage,
        };
    }
}
