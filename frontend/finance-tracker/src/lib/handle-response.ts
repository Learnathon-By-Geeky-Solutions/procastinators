import { toast } from "sonner";

export function handleResponse(
    res: any,
    form: any,
    setOpen: (value: boolean) => void,
    successMessage: string
) {
    if (res.success) {
        toast.success("Success!", {
            description: successMessage,
        });
        form.reset();
        setOpen(false);
    } else {
        const fieldErrors: Record<string, string[]> = res.fieldErrors;
        console.log(fieldErrors);
        for (const [key, value] of Object.entries(fieldErrors)) {
            form.setError(key as keyof typeof fieldErrors, {
                message: value[0],
            });
        }

        toast.error("Failed!", {
            description: res.message,
        });
    }
}
