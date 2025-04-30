import { RegistrationForm } from "@/components/auth/registration-form";

export default async function LoginPage() {
    return (
        <div className="flex min-h-[75vh] w-full items-start justify-center px-4">
            <div className="w-full space-y-8">
                <RegistrationForm />
            </div>
        </div>
    );
}
