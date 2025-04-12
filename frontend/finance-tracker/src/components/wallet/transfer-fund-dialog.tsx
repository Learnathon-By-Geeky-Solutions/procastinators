"use client";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";

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
import { Wallet } from "@/lib/definitions";
import { ArrowRightLeft } from "lucide-react";
import { useEffect, useState } from "react";
import { transferFundFormSchema } from "@/validations/form-schema";
import { TransferFundAction } from "@/lib/actions/wallet-action";
import { toast } from "sonner";

const successTitle = "Success!";
const successDescription = "Fund transferred successfully.";
const failedTitle = "Failed!";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function TransferFundDialog({
    sourceWallet,
    destinationWallets,
}: {
    sourceWallet: Wallet;
    destinationWallets: Wallet[];
}) {
    const [open, setOpen] = useState(false);

    const defaultValues = {
        sourceWalletId: sourceWallet.id,
        destinationWalletId: "",
        amount: "",
    };
    const form = useForm<z.infer<typeof transferFundFormSchema>>({
        resolver: zodResolver(transferFundFormSchema),
        defaultValues,
    });

    const { isSubmitting } = form.formState;

    useEffect(() => {
        form.reset(defaultValues);
    }, [open]);

    async function onSubmit(values: z.infer<typeof transferFundFormSchema>) {
        try {
            const res = await TransferFundAction(values);
            if (res.success) {
                toast.success(successTitle, {
                    description: successDescription,
                });
                form.reset();
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
            <DialogTrigger asChild>
                <Button
                    className="w-full flex items-center justify-center gap-2"
                    variant={"outline"}
                >
                    <ArrowRightLeft className="h-4 w-4" />
                    Transfer
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Transfer Fund</DialogTitle>
                    <DialogDescription>
                        Transfer fund to another wallet.
                    </DialogDescription>
                </DialogHeader>
                <Form {...form}>
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-4 py-4"
                    >
                        {/* Hidden field for source wallet ID */}
                        <FormField
                            control={form.control}
                            name="sourceWalletId"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>From</FormLabel>
                                    <FormControl>
                                        <Input type="hidden" {...field} />
                                    </FormControl>
                                </FormItem>
                            )}
                        />

                        <div className="flex items-center justify-between rounded-lg border p-3 shadow-sm">
                            <div>
                                <p className="text-sm font-medium">
                                    {sourceWallet.name}
                                </p>
                                <p className="text-sm text-muted-foreground">
                                    {sourceWallet.type}
                                </p>
                            </div>
                            <div className="text-right">
                                <p className="text-sm font-medium">Balance</p>
                                <p className="text-sm">
                                    {`${sourceWallet.balance.toFixed(2)} ${
                                        sourceWallet.currency
                                    }`}
                                </p>
                            </div>
                        </div>

                        <FormField
                            control={form.control}
                            name="destinationWalletId"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>To</FormLabel>
                                    <Select
                                        onValueChange={field.onChange}
                                        value={field.value}
                                    >
                                        <FormControl className="w-full">
                                            <SelectTrigger>
                                                <SelectValue placeholder="Select destination wallet" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            {destinationWallets.length > 0 ? (
                                                destinationWallets.map(
                                                    (wallet) => (
                                                        <SelectItem
                                                            key={wallet.id}
                                                            value={wallet.id.toString()}
                                                        >
                                                            {wallet.name}
                                                        </SelectItem>
                                                    )
                                                )
                                            ) : (
                                                <SelectItem value="" disabled>
                                                    No other wallets available
                                                </SelectItem>
                                            )}
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
                            name="amount"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Amount </FormLabel>
                                    <FormControl>
                                        <Input
                                            type="number"
                                            placeholder="0.00"
                                            {...field}
                                        />
                                    </FormControl>

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
                                onClick={() => setOpen(false)}
                            >
                                Cancel
                            </Button>
                            <Button type="submit" disabled={isSubmitting}>
                                {isSubmitting ? "Processing..." : "Transfer"}
                            </Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
