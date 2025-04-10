"use client";

import { z } from "zod";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";

import { Button } from "@/components/ui/button";
import {
    Dialog,
    DialogContent,
    DialogDescription,
    DialogFooter,
    DialogHeader,
    DialogTitle,
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
import { Wallet } from "@/lib/definitions";
import { editWalletFormSchema } from "@/validations/form-schema";
import { toast } from "sonner";
import { UpdateWalletAction } from "@/lib/actions/wallet-action";
import { useEffect } from "react";

const successTitle = "Saved Changes";
const successDescription = "Wallet updated successfully.";
const failedTitle = "Saving Changes Failed";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function EditWalletDialog({
    wallet,
    open,
    setOpen,
}: {
    wallet: Wallet;
    open: boolean;
    setOpen: (value: boolean) => void;
}) {
    const form = useForm<z.infer<typeof editWalletFormSchema>>({
        resolver: zodResolver(editWalletFormSchema),
        defaultValues: {
            id: wallet.id,
            name: wallet.name,
            type: wallet.type,
            currency: wallet.currency,
        },
    });
    const { isSubmitting } = form.formState;

    useEffect(() => {
        form.reset({
            id: wallet.id,
            name: wallet.name,
            type: wallet.type,
            currency: wallet.currency,
        });
    }, [open]);

    async function onSubmit(values: z.infer<typeof editWalletFormSchema>) {
        try {
            const res = await UpdateWalletAction(values);
            if (res.success) {
                toast.success(successTitle, {
                    description: successDescription,
                });
                setOpen(false);
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
        <Dialog open={open} onOpenChange={setOpen}>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Edit Wallet</DialogTitle>
                    <DialogDescription>
                        Update your wallet details.
                    </DialogDescription>
                </DialogHeader>
                <Form {...form}>
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-4 py-4"
                    >
                        {/* Hidden field for wallet ID */}
                        <FormField
                            control={form.control}
                            name="id"
                            render={({ field }) => (
                                <FormItem className="hidden">
                                    <FormControl>
                                        <Input type="hidden" {...field} />
                                    </FormControl>
                                </FormItem>
                            )}
                        />

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
                                        defaultValue={field.value}
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
                                onClick={() => setOpen(false)}
                            >
                                Cancel
                            </Button>
                            <Button type="submit">
                                {isSubmitting ? "Saving" : "Save"}
                            </Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
