"use client";

import { useEffect, useState } from "react";
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
import { Form } from "@/components/ui/form";
import { CommonFields } from "@/components/wallet/common-fields";
import { addWalletFormSchema } from "@/validations/form-schema";
import { AddWalletAction } from "@/lib/actions/wallet-action";
import { toast } from "sonner";
import { handleResponse } from "@/lib/handle-response";

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

    useEffect(() => {
        form.reset();
    }, [open]);

    async function onSubmit(values: z.infer<typeof addWalletFormSchema>) {
        try {
            const res = await AddWalletAction(values);
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
                <Button variant="outline" className="h-12 w-12 rounded-full">
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
                        <CommonFields form={form} />

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
                                {isSubmitting ? "Adding" : "Add"}
                            </Button>
                        </DialogFooter>
                    </form>
                </Form>
            </DialogContent>
        </Dialog>
    );
}
