"use client";

import Link from "next/link";
import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { toast } from "sonner";

import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form";
import { Button } from "@/components/ui/button";
import {
    Card,
    CardContent,
    CardDescription,
    CardFooter,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { PasswordInput } from "@/components/ui/password-input";
import { Loader2Icon, LogInIcon } from "lucide-react";
import { useRouter } from "next/navigation";
import { registrationFormSchema } from "@/validations/form-schema";
import { RegisterUser } from "@/lib/actions/user-action";

const successTitle = "Registration Successful";
const successDescription = "You can now login to your account.";
const failedTitle = "Registration Failed";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function RegistrationForm() {
    const form = useForm<z.infer<typeof registrationFormSchema>>({
        resolver: zodResolver(registrationFormSchema),
        defaultValues: {
            email: "",
            password: "",
            confirmPassword: "",
        },
    });
    const { isSubmitting } = form.formState;

    const router = useRouter();

    async function onSubmit(values: z.infer<typeof registrationFormSchema>) {
        try {
            const res = await RegisterUser(values);
            if (res.success) {
                form.reset();
                toast.success(successTitle, {
                    description: successDescription,
                });

                router.push("/auth/login");
            } else {
                const fieldErrors = res.fieldErrors;
                for (const [key, value] of Object.entries(fieldErrors)) {
                    form.setError(key as keyof typeof fieldErrors, {
                        message: value[0],
                    });
                }
                toast.error(failedTitle, {
                    description: res.message,
                });
            }
        } catch (error) {
            console.error(error);
            toast.error(failedTitle, {
                description: failedDefaultDescription,
            });
        }
    }

    return (
        <Card className="mx-auto max-w-md">
            <CardHeader>
                <CardTitle className="text-2xl">Register</CardTitle>
                <CardDescription>
                    Create an account to start tracking your finances.
                </CardDescription>
            </CardHeader>
            <CardContent>
                <Form {...form}>
                    <form
                        className="space-y-8"
                        onSubmit={form.handleSubmit(onSubmit)}
                    >
                        <div className="grid gap-2">
                            <FormField
                                control={form.control}
                                name="email"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel htmlFor="email">
                                            Email
                                        </FormLabel>
                                        <FormControl>
                                            <Input
                                                id="email"
                                                placeholder="user@example.com"
                                                type="email"
                                                autoComplete="email"
                                                {...field}
                                            />
                                        </FormControl>
                                        <div className="min-h-[20px]">
                                            <FormMessage />
                                        </div>
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="password"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel htmlFor="password">
                                            Password
                                        </FormLabel>
                                        <FormControl>
                                            <PasswordInput
                                                id="password"
                                                placeholder="******"
                                                autoComplete="new-password"
                                                {...field}
                                            />
                                        </FormControl>
                                        <div className="min-h-[20px]">
                                            <FormMessage />
                                        </div>
                                    </FormItem>
                                )}
                            />

                            <FormField
                                control={form.control}
                                name="confirmPassword"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel htmlFor="confirmPassword">
                                            Confirm Password
                                        </FormLabel>
                                        <FormControl>
                                            <PasswordInput
                                                id="confirmPassword"
                                                placeholder="******"
                                                autoComplete="new-password"
                                                {...field}
                                            />
                                        </FormControl>
                                        <div className="min-h-[20px]">
                                            <FormMessage />
                                        </div>
                                    </FormItem>
                                )}
                            />

                            <Button
                                type="submit"
                                className="w-full"
                                disabled={isSubmitting}
                            >
                                {isSubmitting ? (
                                    <div className="flex items-center">
                                        <Loader2Icon className="animate-spin mr-2 h-4 w-4" />
                                        Registering...
                                    </div>
                                ) : (
                                    <div className="flex items-center">
                                        <LogInIcon className="mr-2 h-4 w-4" />
                                        Register
                                    </div>
                                )}
                            </Button>
                        </div>
                    </form>
                </Form>
            </CardContent>
            <CardFooter>
                <div className="w-full text-center text-sm">
                    Already have an account?{" "}
                    <Link href="/auth/login" className="underline">
                        Login
                    </Link>
                </div>
            </CardFooter>
        </Card>
    );
}
