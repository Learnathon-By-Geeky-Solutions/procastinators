"use client";

import { useEffect, useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { HandCoinsIcon } from "lucide-react";
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
import {
    claimInstallmentFormSchema,
    claimLoanFormSchema,
} from "@/validations/form-schema";
import { toast } from "sonner";
import { handleResponse } from "@/lib/handle-response";
import { Input } from "@/components/ui/input";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import { InstallmentClaim, LoanClaim, Wallet } from "@/lib/definitions";
import {
    ClaimInstallmentAction,
    ClaimLoanAction,
} from "@/lib/actions/claim-action";

const successDescription = "Installment claimed successfully.";
const failedTitle = "Failed!";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function ClaimInstallmentDialog({
    wallets,
    claim,
}: {
    readonly wallets: Wallet[];
    readonly claim: InstallmentClaim;
    readonly iconOnly?: boolean;
}) {
    const [open, setOpen] = useState(false);

    const defaultValues = {
        id: claim.id,
        walletId: "",
    };

    const form = useForm<z.infer<typeof claimInstallmentFormSchema>>({
        resolver: zodResolver(claimInstallmentFormSchema),
        defaultValues,
    });
    const { isSubmitting } = form.formState;

    useEffect(() => {
        form.reset(defaultValues);
    }, [open]);

    async function onSubmit(
        values: z.infer<typeof claimInstallmentFormSchema>
    ) {
        try {
            const res = await ClaimInstallmentAction(values);
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
            <DialogTrigger asChild>
                <Button variant="outline" size="sm">
                    <HandCoinsIcon className="h-4 w-4" />
                    Claim
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Claim Installment</DialogTitle>
                    <DialogDescription>
                        Funds will be transferred to the selected wallet.
                    </DialogDescription>
                </DialogHeader>
                <Form {...form}>
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-4 py-4"
                    >
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

                        <div className="flex items-center justify-between rounded-lg border p-3 shadow-sm">
                            <p className="text-sm font-medium">Amount</p>
                            <p className="text-sm">
                                {claim?.installment?.amount.toFixed(2)} BDT
                            </p>
                        </div>

                        <FormField
                            control={form.control}
                            name="walletId"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Wallet</FormLabel>
                                    <Select
                                        onValueChange={field.onChange}
                                        value={field.value}
                                    >
                                        <FormControl className="w-full">
                                            <SelectTrigger>
                                                <SelectValue placeholder="Select wallet" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            {wallets.length > 0 ? (
                                                wallets.map((wallet) => (
                                                    <SelectItem
                                                        key={wallet.id}
                                                        value={wallet.id.toString()}
                                                    >
                                                        {wallet.name}
                                                    </SelectItem>
                                                ))
                                            ) : (
                                                <SelectItem value="0" disabled>
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

                        <DialogFooter className="pt-4">
                            <Button
                                variant="outline"
                                type="button"
                                disabled={isSubmitting}
                                onClick={() => {
                                    setOpen(false);
                                }}
                            >
                                Cancel
                            </Button>
                            <Button type="submit" disabled={isSubmitting}>
                                {isSubmitting ? "Claiming" : "Claim"}
                            </Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
