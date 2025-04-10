"use client";

import { useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { PlusIcon } from "lucide-react";
import { Button } from "@/components/ui/button";
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
    DialogTrigger,
} from "@/components/ui/dialog";
import {
    Form,
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import { addWalletFormSchema } from "@/validations/form-schema";
import { AddWalletAction } from "@/lib/actions/wallet-action";
import { toast, Toaster } from "sonner";

const successTitle = "Success!";
const successDescription = "Wallet added successfully.";
const failedTitle = "Failed!";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function AddWalletDialog() {
    const [open, setOpen] = useState(false);

    const form = useForm<z.infer<typeof addWalletFormSchema>>({
        resolver: zodResolver(addWalletFormSchema),
        defaultValues: {
            name: "",
            type: "",
            currency: "",
        },
    });
    const { isSubmitting } = form.formState;

    const setOpenAndReset = (value: boolean) => {
        setOpen(value);
        form.reset();
    };

    async function onSubmit(values: z.infer<typeof addWalletFormSchema>) {
        try {
            const res = await AddWalletAction(values);
            if (res.success) {
                toast.success(successTitle, {
                    description: successDescription,
                });
                setOpenAndReset(false);
            } else {
                const fieldErrors = res.fieldErrors;
                console.log(fieldErrors);
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
            console.log(error);
            toast.error(failedTitle, {
                description: failedDefaultDescription,
            });
        }
    }

    return (
        <>
            <Dialog open={open} onOpenChange={setOpenAndReset}>
                <DialogTrigger asChild>
                    <Button
                        variant="outline"
                        className="h-12 w-12 rounded-full"
                    >
                        <PlusIcon className="h-6 w-6" />
                    </Button>
                </DialogTrigger>
                <DialogContent className="sm:max-w-[425px]">
                    <DialogHeader>
                        <DialogTitle>New Wallet</DialogTitle>
                        <DialogDescription>
                            Add a new wallet to your account
                        </DialogDescription>
                    </DialogHeader>
                    <Form {...form}>
                        <form
                            onSubmit={form.handleSubmit(onSubmit)}
                            className="space-y-4 py-4"
                        >
                            <FormField
                                control={form.control}
                                name="name"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Name</FormLabel>
                                        <FormControl>
                                            <Input
                                                placeholder="My Wallet"
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
                                name="type"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Type</FormLabel>
                                        <Select
                                            onValueChange={field.onChange}
                                            value={field.value}
                                        >
                                            <FormControl className="w-full">
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Select wallet type" />
                                                </SelectTrigger>
                                            </FormControl>
                                            <SelectContent>
                                                <SelectItem value="Cash">
                                                    Cash
                                                </SelectItem>
                                                <SelectItem value="Bank">
                                                    Bank
                                                </SelectItem>
                                                <SelectItem value="MFS">
                                                    MFS
                                                </SelectItem>
                                            </SelectContent>
                                        </Select>
                                        <div className="min-h-[20px]">
                                            <FormMessage />
                                        </div>
                                    </FormItem>
                                )}
                            />
                            <FormField
                                control={form.control}
                                name="currency"
                                render={({ field }) => (
                                    <FormItem>
                                        <FormLabel>Currency</FormLabel>
                                        <Select
                                            onValueChange={field.onChange}
                                            defaultValue={field.value}
                                        >
                                            <FormControl className="w-full">
                                                <SelectTrigger>
                                                    <SelectValue placeholder="Select currency" />
                                                </SelectTrigger>
                                            </FormControl>
                                            <SelectContent>
                                                <SelectItem value="USD">
                                                    USD
                                                </SelectItem>
                                                <SelectItem value="BDT">
                                                    BDT
                                                </SelectItem>
                                            </SelectContent>
                                        </Select>
                                        <div className="min-h-[20px]">
                                            <FormMessage />
                                        </div>
                                    </FormItem>
                                )}
                            />
                            <DialogFooter className="pt-4">
                                <Button
                                    variant="outline"
                                    type="button"
                                    disabled={isSubmitting}
                                    onClick={() => {
                                        setOpenAndReset(false);
                                    }}
                                >
                                    Cancel
                                </Button>
                                <Button type="submit" disabled={isSubmitting}>
                                    {isSubmitting ? "Adding" : "Add"}
                                </Button>
                            </DialogFooter>
                        </form>
                    </Form>
                </DialogContent>
            </Dialog>
            <Toaster position="bottom-right" richColors />
        </>
    );
}
