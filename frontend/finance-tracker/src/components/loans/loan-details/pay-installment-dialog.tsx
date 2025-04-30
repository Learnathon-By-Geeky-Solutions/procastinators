"use client";

import { useEffect, useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { HandCoinsIcon, PlusIcon } from "lucide-react";
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
    addLoanFormSchema,
    payInstallmentFormSchema,
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
import { Loan, Wallet } from "@/lib/definitions";
import { Textarea } from "@/components/ui/textarea";
import { DateTimePicker } from "@/components/ui/date-time-picker";
import { PayInstallmentAction } from "@/lib/actions/installment-action";

const successDescription = "Transaction processed successfully.";
const failedTitle = "Failed!";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function PayInsallmentDialog({
    action,
    wallets,
    loan,
}: {
    readonly action: "pay" | "receive";
    readonly wallets: Wallet[];
    readonly loan: Loan;
}) {
    const [open, setOpen] = useState(false);

    const defaultValues = {
        action,
        loanId: loan.id,
        dueAmount: loan.dueAmount.toFixed(2),
        walletId: "",
        amount: "",
        nextDueDate: new Date(Date.now() + 1000 * 60 * 60 * 24),
        note: "",
    };

    const form = useForm<z.infer<typeof payInstallmentFormSchema>>({
        resolver: zodResolver(payInstallmentFormSchema),
        defaultValues,
    });
    const { isSubmitting } = form.formState;

    const [currency, setCurrency] = useState("???");

    useEffect(() => {
        form.reset(defaultValues);
    }, [open]);

    async function onSubmit(values: z.infer<typeof payInstallmentFormSchema>) {
        try {
            const res = await PayInstallmentAction(values);
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
                <Button>
                    {action == "pay"
                        ? "Pay Installment"
                        : "Receive Installment"}
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>New Installment</DialogTitle>
                    <DialogDescription>
                        {action === "pay"
                            ? "Pay an installment for this loan."
                            : "Receive an installment for this loan."}
                    </DialogDescription>
                </DialogHeader>
                <Form {...form}>
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-4 py-4"
                    >
                        <FormField
                            control={form.control}
                            name="action"
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
                            name="loanId"
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
                            name="dueAmount"
                            render={({ field }) => (
                                <FormItem className="hidden">
                                    <FormControl>
                                        <Input type="hidden" {...field} />
                                    </FormControl>
                                </FormItem>
                            )}
                        />

                        <div className="flex items-center justify-between rounded-lg border p-3 shadow-sm">
                            <p className="text-sm font-medium">Due Amount</p>
                            <p className="text-sm">
                                {loan.dueAmount.toFixed(2)} BDT
                            </p>
                        </div>

                        <FormField
                            control={form.control}
                            name="walletId"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Wallet</FormLabel>
                                    <Select
                                        onValueChange={(val) => {
                                            setCurrency(
                                                wallets.find((w) => w.id == val)
                                                    ?.currency ?? "???"
                                            );
                                            field.onChange(val);
                                        }}
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

                        <FormField
                            control={form.control}
                            name="amount"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Amount</FormLabel>
                                    <FormControl>
                                        <div className="relative">
                                            <Input
                                                type="number"
                                                placeholder="0.00"
                                                {...field}
                                            />
                                            <span className="absolute right-8 top-1/2 -translate-y-1/2 text-muted-foreground">
                                                {currency}
                                            </span>
                                        </div>
                                    </FormControl>

                                    <div className="min-h-[20px]">
                                        <FormMessage />
                                    </div>
                                </FormItem>
                            )}
                        />

                        <FormField
                            control={form.control}
                            name="nextDueDate"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Date & Time</FormLabel>
                                    <FormControl>
                                        <DateTimePicker
                                            date={field.value}
                                            setDate={field.onChange}
                                            modal={true}
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
                            name="note"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>
                                        Note
                                        <span className="font-normal text-muted-foreground">
                                            (Optional)
                                        </span>
                                    </FormLabel>
                                    <FormControl>
                                        <Textarea
                                            placeholder="About this transaction"
                                            className="resize-none"
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
                                onClick={() => {
                                    setOpen(false);
                                }}
                            >
                                Cancel
                            </Button>
                            {action === "pay" ? (
                                <Button type="submit" disabled={isSubmitting}>
                                    {isSubmitting ? "Paying" : "Pay"}
                                </Button>
                            ) : (
                                <Button type="submit" disabled={isSubmitting}>
                                    {isSubmitting ? "Receiving" : "Receive"}
                                </Button>
                            )}
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
