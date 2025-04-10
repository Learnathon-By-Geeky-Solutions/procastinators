import { z } from "zod";

export const registrationFormSchema = z
    .object({
        email: z
            .string()
            .min(1, { message: "Email is required" })
            .email({ message: "Invalid email address" }),
        password: z
            .string()
            .min(1, { message: "Password is required" })
            .min(6, { message: "Password must be at least 6 characters long" })
            .refine((password) => /[!@#$%^&*(),.?":{}|<>]/.test(password), {
                message:
                    "Password must have at least one non alphanumeric character.",
            })
            .refine((password) => /\d/.test(password), {
                message: "Password must have at least one digit ('0'-'9').",
            })
            .refine((password) => /[a-z]/.test(password), {
                message: "Password must have at least one lowercase ('a'-'z').",
            })
            .refine((password) => /[A-Z]/.test(password), {
                message: "Password must have at least one uppercase ('A'-'Z').",
            }),
        confirmPassword: z
            .string()
            .min(1, { message: "Password confirmation is required" }),
    })
    .refine((data) => data.password === data.confirmPassword, {
        path: ["confirmPassword"],
        message: "Passwords do not match",
    });
