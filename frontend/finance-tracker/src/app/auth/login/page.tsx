import { LoginForm } from "@/components/auth/login-form";

export default async function LoginPage() {
    return (
        <div className="flex min-h-[75vh] w-full items-start justify-center px-4">
            <div className="w-full space-y-8">
                <LoginForm />
            </div>
        </div>
    );
}
