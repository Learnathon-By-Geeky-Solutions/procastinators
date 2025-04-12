"use server";

import {
    addWalletFormSchema,
    editWalletFormSchema,
    transferFundFormSchema,
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

async function handleFetch(httpMethod: string, url: string, body: any) {
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

    const { name, type, currency } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/wallets`;
    return handleFetch("POST", url, {
        name,
        type,
        currency,
    });
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

    const { id, name, type, currency } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/wallets/${id}`;
    return handleFetch("PATCH", url, {
        name,
        type,
        currency,
    });
}

export async function DeleteWalletAction(id: string) {
    const url = `${process.env.BACKEND_BASE_URL}/wallets/${id}`;
    return await handleFetch("DELETE", url, {});
}

export async function TransferFundAction(
    formData: z.infer<typeof transferFundFormSchema>
) {
    const validatedFields = transferFundFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const { sourceWalletId, destinationWalletId, amount } =
        validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/wallets/${sourceWalletId}/transfer`;
    return await handleFetch("POST", url, {
        destinationWalletId,
        amount,
    });
}
