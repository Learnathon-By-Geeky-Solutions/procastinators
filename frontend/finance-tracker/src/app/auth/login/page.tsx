import { LoginForm } from "@/components/auth/login-form";

export default async function LoginPage() {
    return (
        <div className="flex min-h-[75vh] h-full w-full items-center justify-center px-4">
            <div className="w-full space-y-8">
                <div className="text-center">
                    <h1 className="text-3xl font-bold tracking-tight">
                        Finance Tracker
                    </h1>
                </div>
                <LoginForm />
            </div>
        </div>
    );
}
