"use server";

import {
    approveLoanRequestFormSchema,
    requestLoanFormSchema,
} from "@/validations/form-schema";
import { z } from "zod";
import { handleAction } from "@/lib/actions/handle-action";

export async function RequestLoanAction(
    formData: z.infer<typeof requestLoanFormSchema>
) {
    const validatedFields = requestLoanFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const { lenderId, amount, dueDate, note } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/loanrequests`;
    return handleAction("POST", url, {
        lenderId,
        amount,
        note,
        dueDate: dueDate.toISOString(),
    });
}

export async function ApproveLoanRequestAction(
    formData: z.infer<typeof approveLoanRequestFormSchema>
) {
    const validatedFields = approveLoanRequestFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    const { id, lenderWalletId } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/loanrequests/${id}/approve`;
    return handleAction("POST", url, {
        lenderWalletId,
    });
}
