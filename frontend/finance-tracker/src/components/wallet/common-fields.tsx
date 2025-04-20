import {
    FormControl,
    FormField,
    FormItem,
    FormLabel,
    FormMessage,
} from "@/components/ui/form";
import {
    Select,
    SelectContent,
    SelectItem,
    SelectTrigger,
    SelectValue,
} from "@/components/ui/select";
import { Input } from "@/components/ui/input";

export function CommonFields({ form }: { readonly form: any }) {
    return (
        <>
            <FormField
                control={form.control}
                name="name"
                render={({ field }) => (
                    <FormItem>
                        <FormLabel>Name</FormLabel>
                        <FormControl>
                            <Input placeholder="My Wallet" {...field} />
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
                            value={field.value}
                        >
                            <FormControl className="w-full">
                                <SelectTrigger>
                                    <SelectValue placeholder="Select wallet type" />
                                </SelectTrigger>
                            </FormControl>
                            <SelectContent>
                                <SelectItem value="Cash">Cash</SelectItem>
                                <SelectItem value="Bank">Bank</SelectItem>
                                <SelectItem value="MFS">MFS</SelectItem>
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
                                <SelectItem value="BDT">BDT</SelectItem>
                            </SelectContent>
                        </Select>
                        <div className="min-h-[20px]">
                            <FormMessage />
                        </div>
                    </FormItem>
                )}
            />
        </>
    );
}
