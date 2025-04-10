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
import { Form, FormControl, FormField, FormItem } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Wallet } from "@/lib/definitions";
import { deleteWalletFormSchema } from "@/validations/form-schema";
import { toast } from "sonner";
import { DeleteWalletAction } from "@/lib/actions/wallet-action";

const successTitle = "Successful";
const successDescription = "Wallet has been deleted successfully.";
const failedTitle = "Failed";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function DeleteWalletDialog({
    wallet,
    open,
    setOpen,
}: {
    wallet: Wallet;
    open: boolean;
    setOpen: (value: boolean) => void;
}) {
    const form = useForm<z.infer<typeof deleteWalletFormSchema>>({
        resolver: zodResolver(deleteWalletFormSchema),
        defaultValues: {
            id: wallet.id,
        },
    });
    const { isSubmitting } = form.formState;

    async function onSubmit(values: z.infer<typeof deleteWalletFormSchema>) {
        try {
            const res = await DeleteWalletAction(values.id);
            if (res.success) {
                toast.success(successTitle, {
                    description: successDescription,
                });
                form.reset();
                setOpen(false);
            } else {
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
                    <DialogTitle>Delete Wallet</DialogTitle>
                    <DialogDescription>
                        Are you sure you want to delete this wallet? You won't
                        be able to use this wallet in the future but it may
                        still be visible in your transaction history.
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

                        <DialogFooter className="pt-4">
                            <Button
                                variant="outline"
                                type="button"
                                disabled={isSubmitting}
                                onClick={() => setOpen(false)}
                            >
                                Cancel
                            </Button>
                            <Button
                                type="submit"
                                variant="destructive"
                                disabled={isSubmitting}
                            >
                                {isSubmitting ? "Deleting" : "Delete"}
                            </Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
