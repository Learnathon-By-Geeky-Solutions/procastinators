"use server";

import {
    addWalletFormSchema,
    editWalletFormSchema,
} from "@/validations/form-schema";
import { z } from "zod";
import { auth } from "@/lib/auth";
import { revalidatePath } from "next/cache";

const defaultErrorMessage = "Something went wrong";

function MapFieldErrors(errors: Record<string, string[]>) {
    return Object.entries(errors ?? {}).reduce((obj, [key, value]) => {
        obj[key.toLocaleLowerCase()] = Array.isArray(value) ? value : [value];
        return obj;
    }, {} as Record<string, string[]>);
}

export async function AddWalletAction(
    formData: z.infer<typeof addWalletFormSchema>
) {
    const validatedFields = addWalletFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    try {
        const { name, type, currency } = validatedFields.data;
        const url = `${process.env.BACKEND_BASE_URL}/wallets`;
        const session = await auth();
        const res = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${session?.accessToken}`,
            },
            body: JSON.stringify({
                name,
                type,
                currency,
            }),
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

export async function UpdateWalletAction(
    formData: z.infer<typeof editWalletFormSchema>
) {
    const validatedFields = editWalletFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    try {
        const { id, name, type, currency } = validatedFields.data;
        const url = `${process.env.BACKEND_BASE_URL}/wallets/${id}`;
        const session = await auth();
        const res = await fetch(url, {
            method: "PATCH",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${session?.accessToken}`,
            },
            body: JSON.stringify({
                name,
                type,
                currency,
            }),
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

export async function DeleteWalletAction(id: string) {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/wallets/${id}`;
        const session = await auth();
        const res = await fetch(url, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                Authorization: `Bearer ${session?.accessToken}`,
            },
        });

        if (!res.ok) {
            const data = await res.json();
            console.log(data);

            return {
                success: false,
                fieldErrors: {},
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
