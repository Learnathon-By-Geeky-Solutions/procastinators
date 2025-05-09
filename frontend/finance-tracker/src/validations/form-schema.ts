import { z } from "zod";

export const loginFormSchema = z.object({
    email: z
        .string()
        .min(1, { message: "Email is required" })
        .email({ message: "Invalid email address" }),
    password: z.string().min(1, { message: "Password is required" }),
});

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

export const addWalletFormSchema = z.object({
    name: z.string().min(1, {
        message: "Wallet name required",
    }),
    type: z.string().min(1, {
        message: "Wallet type is required",
    }),
    currency: z.string().min(1, {
        message: "Currency is required",
    }),
});

export const editWalletFormSchema = addWalletFormSchema.extend({
    id: z.coerce.string().min(1, {
        message: "ID is required",
    }),
});

export const deleteWalletFormSchema = editWalletFormSchema.omit({
    name: true,
    type: true,
    currency: true,
});

export const transferFundFormSchema = z.object({
    amount: z.coerce
        .string()
        .min(1, {
            message: "Amount is required",
        })
        .refine(
            (value) => {
                const amount = parseFloat(value);
                return !isNaN(amount) && amount > 0;
            },
            {
                message: "Amount must be a greater than 0",
            }
        ),
    sourceWalletId: z.coerce.string().min(1, {
        message: "Source wallet is required",
    }),
    destinationWalletId: z.coerce.string().min(1, {
        message: "Destination wallet is required",
    }),
});

export const addCategoryFormSchema = z.object({
    title: z.string().min(1, {
        message: "Category title is required",
    }),
    defaultTransactionType: z.string().min(1, {
        message: "Default transaction type is required",
    }),
});
export const editCategoryFormSchema = addCategoryFormSchema.extend({
    id: z.coerce.string().min(1, {
        message: "ID is required",
    }),
});
export const deleteCategoryFormSchema = editCategoryFormSchema.omit({
    title: true,
    defaultTransactionType: true,
});

export const addTransactionFormSchema = z.object({
    categoryId: z.coerce.string().min(1, {
        message: "Category is required",
    }),
    transactionType: z.coerce.string().min(1, {
        message: "Transaction type is required",
    }),
    walletId: z.coerce.string().min(1, {
        message: "Wallet is required",
    }),
    amount: z.coerce
        .string()
        .min(1, {
            message: "Amount is required",
        })
        .refine(
            (value) => {
                const amount = parseFloat(value);
                return !isNaN(amount) && amount > 0;
            },
            {
                message: "Amount must be a greater than 0",
            }
        ),
    timestamp: z.date({
        required_error: "Date & time is required",
    }),
    note: z.coerce.string().optional(),
});

export const editTransactionFormSchema = addTransactionFormSchema.extend({
    id: z.coerce.string().min(1, {
        message: "ID is required",
    }),
});
export const deleteTransactionFormSchema = editTransactionFormSchema.omit({
    categoryId: true,
    transactionType: true,
    walletId: true,
    amount: true,
    timestamp: true,
    note: true,
});

export const addLoanFormSchema = z.object({
    action: z.enum(["borrow", "lend"], {
        required_error: "Action is required",
    }),
    walletId: z.coerce.string().min(1, {
        message: "Wallet is required",
    }),
    amount: z.coerce
        .string()
        .min(1, {
            message: "Amount is required",
        })
        .refine(
            (value) => {
                const amount = parseFloat(value);
                return !isNaN(amount) && amount > 0;
            },
            {
                message: "Amount must be a greater than 0",
            }
        ),
    dueDate: z
        .date({
            required_error: "Due date is required",
        })
        .refine((date) => date > new Date(Date.now() + 60 * 60 * 1000), {
            message: "Due date must be at least an hour from now",
        }),
    note: z.coerce.string().optional(),
});

export const payInstallmentFormSchema = z
    .object({
        loanId: z.coerce.string().min(1, {
            message: "Loan is required",
        }),
        action: z.enum(["pay", "receive"], {
            required_error: "Action is required",
        }),
        walletId: z.coerce.string().min(1, {
            message: "Wallet is required",
        }),
        dueAmount: z.coerce.string().min(1, {
            message: "Due amount is required",
        }),
        amount: z.coerce
            .string()
            .min(1, {
                message: "Amount is required",
            })
            .refine(
                (value) => {
                    const amount = parseFloat(value);
                    return !isNaN(amount) && amount > 0;
                },
                {
                    message: "Amount must be a greater than 0",
                }
            ),
        nextDueDate: z
            .date({
                required_error: "Due date is required",
            })
            .refine((date) => date > new Date(Date.now() + 60 * 60 * 1000), {
                message: "Due date must be at least an hour from now",
            }),
        note: z.coerce.string().optional(),
    })
    .refine(
        (data) => {
            const amount = parseFloat(data.amount);
            const dueAmount = parseFloat(data.dueAmount);
            return amount <= dueAmount;
        },
        {
            message: "Amount cannot exceed due amount",
            path: ["amount"],
        }
    );

export const requestLoanFormSchema = z.object({
    lenderId: z.coerce.string().min(1, {
        message: "Lender is required",
    }),
    amount: z.coerce
        .string()
        .min(1, {
            message: "Amount is required",
        })
        .refine(
            (value) => {
                const amount = parseFloat(value);
                return !isNaN(amount) && amount > 0;
            },
            {
                message: "Amount must be a greater than 0",
            }
        ),
    dueDate: z
        .date({
            required_error: "Due date is required",
        })
        .refine((date) => date > new Date(Date.now() + 60 * 60 * 1000), {
            message: "Due date must be at least an hour from now",
        }),
    note: z.coerce.string().optional(),
});

export const approveLoanRequestFormSchema = z.object({
    id: z.coerce.string().min(1, {
        message: "Id is required",
    }),
    lenderWalletId: z.coerce.string().min(1, {
        message: "Lender wallet is required",
    }),
});

export const claimLoanFormSchema = z.object({
    id: z.coerce.string().min(1, {
        message: "Id is required",
    }),
    walletId: z.coerce.string().min(1, {
        message: "Wallet is required",
    }),
});

export const claimInstallmentFormSchema = z.object({
    id: z.coerce.string().min(1, {
        message: "Id is required",
    }),
    walletId: z.coerce.string().min(1, {
        message: "Wallet is required",
    }),
});
