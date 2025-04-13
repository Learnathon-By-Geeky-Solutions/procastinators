"use server";

import {
    addWalletFormSchema,
    editWalletFormSchema,
    transferFundFormSchema,
} from "@/validations/form-schema";
import { z } from "zod";
import { handleAction } from "@/lib/actions/handle-action";

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
    return handleAction("POST", url, {
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
    return handleAction("PATCH", url, {
        name,
        type,
        currency,
    });
}

export async function DeleteWalletAction(id: string) {
    const url = `${process.env.BACKEND_BASE_URL}/wallets/${id}`;
    return await handleAction("DELETE", url, {});
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
    return await handleAction("POST", url, {
        destinationWalletId,
        amount,
    });
}
