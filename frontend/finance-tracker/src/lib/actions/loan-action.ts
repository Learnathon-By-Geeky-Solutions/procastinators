"use server";

import { addLoanFormSchema } from "@/validations/form-schema";
import { z } from "zod";
import { handleAction } from "@/lib/actions/handle-action";

export async function AddLoanAction(
    formData: z.infer<typeof addLoanFormSchema>
) {
    const validatedFields = addLoanFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const { action, walletId, amount, note, dueDate } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/loans/${action}`;
    return handleAction("POST", url, {
        walletId,
        amount,
        note,
        dueDate: dueDate.toISOString(),
    });
}
