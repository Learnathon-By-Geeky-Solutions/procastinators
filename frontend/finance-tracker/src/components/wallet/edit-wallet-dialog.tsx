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
import { handleResponse } from "@/lib/handle-response";
import { CommonFields } from "@/components/wallet/common-fields";

const successDescription = "Wallet updated successfully.";
const failedTitle = "Saving Changes Failed";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function EditWalletDialog({
    wallet,
    open,
    setOpen,
}: {
    readonly wallet: Wallet;
    readonly open: boolean;
    readonly setOpen: (value: boolean) => void;
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
            handleResponse(res, form, setOpen, successDescription);
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
                        <CommonFields form={form} />

                        <DialogFooter className="pt-4">
                            <Button
                                variant="outline"
                                type="button"
                                disabled={isSubmitting}
                                onClick={() => setOpen(false)}
                            >
                                Cancel
                            </Button>
                            <Button type="submit" disabled={isSubmitting}>
                                {isSubmitting ? "Saving" : "Save"}
                            </Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
