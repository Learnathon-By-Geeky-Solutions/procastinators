"use server";

import { payInstallmentFormSchema } from "@/validations/form-schema";
import { z } from "zod";
import { handleAction } from "@/lib/actions/handle-action";

export async function PayInstallmentAction(
    formData: z.infer<typeof payInstallmentFormSchema>
) {
    console.log("PayInstallmentAction", formData);
    const validatedFields = payInstallmentFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const { action, loanId, walletId, amount, nextDueDate, note } =
        validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/loans/${loanId}/installments/${action}`;
    return handleAction("POST", url, {
        walletId,
        amount,
        nextDueDate: nextDueDate.toISOString(),
        note,
    });
}
