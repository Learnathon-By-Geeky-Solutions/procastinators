"use server";

import {
    addTransactionFormSchema,
    editTransactionFormSchema,
} from "@/validations/form-schema";
import { z } from "zod";
import { handleAction } from "@/lib/actions/handle-action";

export async function AddTransactionAction(
    formData: z.infer<typeof addTransactionFormSchema>
) {
    const validatedFields = addTransactionFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const { categoryId, walletId, amount, note, transactionType, timestamp } =
        validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/personaltransactions`;
    console.log(timestamp.toISOString());
    return handleAction("POST", url, {
        categoryId,
        walletId,
        amount,
        note,
        transactionType,
        timestamp: timestamp.toISOString(),
    });
}

export async function EditTransactionAction(
    formData: z.infer<typeof editTransactionFormSchema>
) {
    const validatedFields = editTransactionFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const {
        id,
        categoryId,
        walletId,
        amount,
        note,
        transactionType,
        timestamp,
    } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/personaltransactions/${id}`;
    return handleAction("PATCH", url, {
        categoryId,
        walletId,
        amount,
        note,
        transactionType,
        timestamp: timestamp.toISOString(),
    });
}

export async function DeleteTransactionAction(id: string) {
    const url = `${process.env.BACKEND_BASE_URL}/personaltransactions/${id}`;
    return handleAction("DELETE", url, {});
}
