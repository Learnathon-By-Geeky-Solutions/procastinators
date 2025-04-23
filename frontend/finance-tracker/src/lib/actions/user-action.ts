"use server";

import { z } from "zod";
import {
    loginFormSchema,
    registrationFormSchema,
} from "@/validations/form-schema";
import { signIn } from "@/lib/auth";

const defaultErrorMessage = "Something went wrong";

const fieldErrorMapper = (errors: any) => {
    const fieldErrors: {
        email?: string[];
        password?: string[];
    } = {};
    if (errors?.DuplicateUserName) {
        fieldErrors.email = ["Email is already taken."];
    }
    return fieldErrors;
};

export async function loginUser(formData: z.infer<typeof loginFormSchema>) {
    try {
        return await signIn("credentials", {
            ...formData,
            redirect: false,
        });
    } catch (error) {
        return {
            error: error instanceof Error ? error.message : defaultErrorMessage,
        };
    }
}

export async function RegisterUser(
    formData: z.infer<typeof registrationFormSchema>
) {
    const validatedFields = registrationFormSchema.safeParse(formData);
    if (validatedFields.error) {
        return {
            success: false,
            fieldErrors: validatedFields.error.flatten().fieldErrors,
            message: "Invalid field(s)",
        };
    }

    try {
        const { email, password } = validatedFields.data;
        const url = `${process.env.BACKEND_BASE_URL}/identity/register`;
        const res = await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({
                email,
                password,
            }),
        });

        if (!res.ok) {
            const data = await res.json();
            console.log(data);
            return {
                success: false,
                fieldErrors: { ...fieldErrorMapper(data.errors) },
                message: data?.title ?? defaultErrorMessage,
            };
        }

        return {
            success: true,
            fieldErrors: {},
            message: "",
        };
    } catch (error) {
        return {
            success: false,
            fieldErrors: {},
            message:
                error instanceof Error ? error.message : defaultErrorMessage,
        };
    }
}
