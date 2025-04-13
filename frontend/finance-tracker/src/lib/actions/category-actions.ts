"use server";

import {
    addCategoryFormSchema,
    editCategoryFormSchema,
} from "@/validations/form-schema";
import { z } from "zod";
import { handleAction } from "@/lib/actions/handle-action";

export async function AddCategoryAction(
    formData: z.infer<typeof addCategoryFormSchema>
) {
    const validatedFields = addCategoryFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const { title, defaultTransactionType } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/categories`;
    return handleAction("POST", url, {
        title,
        defaultTransactionType,
    });
}

export async function EditCategoryAction(
    formData: z.infer<typeof editCategoryFormSchema>
) {
    const validatedFields = editCategoryFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const { id, title, defaultTransactionType } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/categories/${id}`;
    return handleAction("PATCH", url, {
        title,
        defaultTransactionType,
    });
}
