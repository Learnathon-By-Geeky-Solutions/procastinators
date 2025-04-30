"use client";

import { useEffect, useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { useForm } from "react-hook-form";
import { z } from "zod";
import { HandCoinsIcon, Search, X } from "lucide-react";
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
import { requestLoanFormSchema } from "@/validations/form-schema";
import { toast } from "sonner";
import { handleResponse } from "@/lib/handle-response";
import { Input } from "@/components/ui/input";
import { Textarea } from "@/components/ui/textarea";
import { DateTimePicker } from "@/components/ui/date-time-picker";
import { Badge } from "../ui/badge";
import { UserInfo } from "@/lib/definitions";
import { fetchUserByEmail } from "@/lib/actions/user-action";
import { RequestLoanAction } from "@/lib/actions/loan-request-action";

const successDescription = "Transaction processed successfully.";
const failedTitle = "Failed!";
const failedDefaultDescription = "Something went wrong. Please try again.";

const mockUsers: UserInfo[] = [
    { id: "user1", email: "john@example.com", userName: "John Smith" },
    { id: "user2", email: "sarah@example.com", userName: "Sarah Johnson" },
    { id: "user3", email: "mike@example.com", userName: "Mike Brown" },
    { id: "user4", email: "emma@example.com", userName: "Emma Wilson" },
    { id: "user5", email: "alex@example.com", userName: "Alex Davis" },
    { id: "user6", email: "lisa@example.com", userName: "Lisa Garcia" },
    { id: "user7", email: "david@example.com", userName: "David Martinez" },
];

export function RequestLoanDialog({
    iconOnly = false,
}: {
    readonly iconOnly?: boolean;
}) {
    const [open, setOpen] = useState(false);
    const [searchQuery, setSearchQuery] = useState("");
    const [searchResults, setSearchResults] = useState<UserInfo[]>([]);
    const [selectedUser, setSelectedUser] = useState<UserInfo | null>(null);
    const [showResults, setShowResults] = useState(false);

    const defaultValues = {
        lenderId: "",
        amount: "",
        dueDate: new Date(Date.now() + 1000 * 60 * 60 * 24),
        note: "",
    };

    const form = useForm<z.infer<typeof requestLoanFormSchema>>({
        resolver: zodResolver(requestLoanFormSchema),
        defaultValues,
    });
    const { isSubmitting } = form.formState;

    const [currency, setCurrency] = useState("BDT");

    useEffect(() => {
        form.reset(defaultValues);
        setSelectedUser(null);
        setSearchQuery("");
    }, [open]);

    // Handle search input changes
    useEffect(() => {
        const handler = setTimeout(async () => {
            if (searchQuery.length > 1) {
                const res = await fetchUserByEmail(searchQuery);
                if (res) setSearchResults([res]);
                else setSearchResults([]);
                setShowResults(true);
            } else {
                setSearchResults([]);
                setShowResults(false);
            }
        }, 500); // 500ms debounce delay

        return () => {
            clearTimeout(handler);
        };
    }, [searchQuery]);

    // Handle clicking outside the search results
    useEffect(() => {
        const handleClickOutside = () => {
            setShowResults(false);
        };

        document.addEventListener("click", handleClickOutside);
        return () => {
            document.removeEventListener("click", handleClickOutside);
        };
    }, []);

    const handleSearchClick = (e: React.MouseEvent) => {
        e.stopPropagation();
        if (searchQuery.length > 1) {
            setShowResults(true);
        }
    };

    const selectUser = (user: UserInfo) => {
        setSelectedUser(user);
        form.setValue("lenderId", user.id);
        setSearchQuery("");
        setShowResults(false);
    };

    const removeSelectedUser = () => {
        setSelectedUser(null);
        form.setValue("lenderId", "");
    };

    async function onSubmit(values: z.infer<typeof requestLoanFormSchema>) {
        try {
            const res = await RequestLoanAction(values);
            handleResponse(res, form, setOpen, successDescription);
            console.log("Submitting loan request:", values);
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
                {iconOnly ? (
                    <Button variant={"outline"} size="icon">
                        <HandCoinsIcon className="h-4 w-4" />
                    </Button>
                ) : (
                    <Button variant={"outline"}>
                        <HandCoinsIcon className="h-4 w-4" />
                        Request Loan
                    </Button>
                )}
            </DialogTrigger>
            <DialogContent className="sm:max-w-[425px]">
                <DialogHeader>
                    <DialogTitle>Request Loan</DialogTitle>
                    <DialogDescription>
                        Request a loan to another Finance Tracker user.
                    </DialogDescription>
                </DialogHeader>
                <Form {...form}>
                    <form
                        onSubmit={form.handleSubmit(onSubmit)}
                        className="space-y-4 py-4"
                    >
                        <FormField
                            control={form.control}
                            name="lenderId"
                            render={({ field }) => (
                                <FormItem>
                                    <FormLabel>Search User</FormLabel>
                                    <div className="space-y-2">
                                        <div className="relative">
                                            {!selectedUser ? (
                                                <>
                                                    <div className="flex gap-2">
                                                        <div
                                                            className="relative flex-1"
                                                            onClick={
                                                                handleSearchClick
                                                            }
                                                        >
                                                            <Search className="absolute left-2.5 top-2.5 h-4 w-4 text-muted-foreground" />
                                                            <Input
                                                                placeholder="Search by email or name"
                                                                value={
                                                                    searchQuery
                                                                }
                                                                onChange={(e) =>
                                                                    setSearchQuery(
                                                                        e.target
                                                                            .value
                                                                    )
                                                                }
                                                                className="pl-8"
                                                                onClick={(e) =>
                                                                    e.stopPropagation()
                                                                }
                                                            />
                                                        </div>
                                                    </div>
                                                    {showResults &&
                                                        searchResults.length >
                                                            0 && (
                                                            <div className="absolute z-10 w-full mt-1 border rounded-md bg-background shadow-md max-h-60 overflow-y-auto">
                                                                {searchResults.map(
                                                                    (user) => (
                                                                        <div
                                                                            key={
                                                                                user.id
                                                                            }
                                                                            className="p-2 hover:bg-muted cursor-pointer border-b last:border-b-0"
                                                                            onClick={() =>
                                                                                selectUser(
                                                                                    user
                                                                                )
                                                                            }
                                                                        >
                                                                            <div className="font-medium">
                                                                                {
                                                                                    user.userName
                                                                                }
                                                                            </div>
                                                                            <div className="text-sm text-muted-foreground">
                                                                                {
                                                                                    user.email
                                                                                }
                                                                            </div>
                                                                        </div>
                                                                    )
                                                                )}
                                                            </div>
                                                        )}
                                                    {showResults &&
                                                        searchResults.length ===
                                                            0 &&
                                                        searchQuery.length >
                                                            1 && (
                                                            <div className="absolute z-10 w-full mt-1 border rounded-md bg-background shadow-md p-2 text-center text-muted-foreground">
                                                                No users found
                                                            </div>
                                                        )}
                                                </>
                                            ) : (
                                                <div className="flex items-center gap-2">
                                                    <Badge
                                                        variant="outline"
                                                        className="flex items-center justify-start gap-2 px-3 h-[36px] w-full"
                                                    >
                                                        <span className="flex-1 items-center gap-2 text-sm">
                                                            {selectedUser.email}
                                                        </span>
                                                        <Button
                                                            size={"icon"}
                                                            variant={"link"}
                                                            className="h-3 w-3 p-2 cursor-pointer hover:text-destructive"
                                                            onClick={
                                                                removeSelectedUser
                                                            }
                                                        >
                                                            <X />
                                                        </Button>
                                                    </Badge>
                                                </div>
                                            )}
                                        </div>
                                        <FormControl>
                                            <Input type="hidden" {...field} />
                                        </FormControl>
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
                            name="dueDate"
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
