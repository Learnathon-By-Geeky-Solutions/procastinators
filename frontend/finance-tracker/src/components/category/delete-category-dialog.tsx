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
    DialogTrigger,
} from "@/components/ui/dialog";
import { Form, FormControl, FormField, FormItem } from "@/components/ui/form";
import { Input } from "@/components/ui/input";
import { Category } from "@/lib/definitions";
import { deleteCategoryFormSchema } from "@/validations/form-schema";
import { toast } from "sonner";
import { useState } from "react";
import { DeleteCategoryAction } from "@/lib/actions/category-actions";
import { Trash2Icon } from "lucide-react";

const successTitle = "Successful";
const successDescription = "Wallet has been deleted successfully.";
const failedTitle = "Failed";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function DeleteCategoryDialog({
    category,
}: {
    readonly category: Category;
}) {
    const [open, setOpen] = useState(false);

    const form = useForm<z.infer<typeof deleteCategoryFormSchema>>({
        resolver: zodResolver(deleteCategoryFormSchema),
        defaultValues: {
            id: category.id,
        },
    });
    const { isSubmitting } = form.formState;

    async function onSubmit(values: z.infer<typeof deleteCategoryFormSchema>) {
        try {
            const res = await DeleteCategoryAction(values.id);
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
            <DialogTrigger asChild>
                <Button variant="ghost" size="icon">
                    <Trash2Icon className="h-4 w-4 text-destructive" />
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Delete Category</DialogTitle>
                    <DialogDescription>
                        Are you sure you want to delete {category.title}? You
                        won't be able to use this category in the future but it
                        may still be visible in your transaction history.
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
