"use server";

import { addCategoryFormSchema } from "@/validations/form-schema";
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
