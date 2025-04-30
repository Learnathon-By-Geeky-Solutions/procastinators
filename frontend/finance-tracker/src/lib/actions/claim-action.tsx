"use server";
import { claimLoanFormSchema } from "@/validations/form-schema";
import { z } from "zod";
import { handleAction } from "@/lib/actions/handle-action";

export async function ClaimLoanAction(
    formData: z.infer<typeof claimLoanFormSchema>
) {
    const validatedFields = claimLoanFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }
    const { id, walletId } = validatedFields.data;
    const url = `${process.env.BACKEND_BASE_URL}/loanclaims/${id}/claim`;
    return handleAction("POST", url, {
        walletId,
    });
}
