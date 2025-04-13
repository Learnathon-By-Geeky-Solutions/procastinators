"use client";

import { useEffect, useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { PencilIcon } from "lucide-react";
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
import { toast } from "sonner";
import { handleResponse } from "@/lib/handle-response";
import { Input } from "../ui/input";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "../ui/select";
import { EditCategoryAction } from "@/lib/actions/category-actions";
import { editCategoryFormSchema } from "@/validations/form-schema";
import { Category } from "@/lib/definitions";

const successDescription = "Category details updated successfully.";
const failedTitle = "Failed!";
const failedDefaultDescription = "Something went wrong. Please try again.";

export function EditCategoryDialog({
    category,
}: {
    readonly category: Category;
}) {
    const [open, setOpen] = useState(false);

    const defaultValues = {
        id: category.id,
        title: category.title,
        defaultTransactionType: category.defaultTransactionType,
    };
    const form = useForm<z.infer<typeof editCategoryFormSchema>>({
        resolver: zodResolver(editCategoryFormSchema),
        defaultValues,
    });
    const { isSubmitting } = form.formState;

    useEffect(() => {
        form.reset(defaultValues);
    }, [open]);

    async function onSubmit(values: z.infer<typeof editCategoryFormSchema>) {
        try {
            const res = await EditCategoryAction(values);
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
                <Button variant="ghost" size="icon">
                    <PencilIcon className="h-4 w-4" />
                </Button>
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Edit Category</DialogTitle>
                    <DialogDescription>
                        Update your category details.
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
                            name="title"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Title</FormLabel>
                                    <FormControl>
                                        <Input
                                            placeholder="Title of the new category"
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
                            name="defaultTransactionType"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Type</FormLabel>
                                    <Select
                                        onValueChange={field.onChange}
                                        value={field.value}
                                    >
                                        <FormControl className="w-full">
                                            <SelectTrigger>
                                                <SelectValue placeholder="Select default transaction type" />
                                            </SelectTrigger>
                                        </FormControl>
                                        <SelectContent>
                                            <SelectItem value="Income">
                                                Income
                                            </SelectItem>
                                            <SelectItem value="Expense">
                                                Expense
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
