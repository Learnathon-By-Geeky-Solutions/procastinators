import type React from "react";
import {
    Activity,
    BadgeDollarSign,
    BarChart3,
    CreditCard,
    HandCoins,
    Users,
} from "lucide-react";
import Link from "next/link";

export default function AuthLayout({
    children,
}: {
    children: React.ReactNode;
}) {
    return (
        <div className="min-h-screen bg-gray-50">
            {/* Header with logo */}
            <header className="border-b">
                <div className="container mx-auto flex h-16 items-center justify-between px-4 md:px-6">
                    <div className="flex items-center gap-2">
                        <Activity className="h-6 w-6 text-primary" />
                        <span className="text-xl font-bold">
                            Finance Tracker
                        </span>
                    </div>
                    <nav className="hidden md:flex gap-6"></nav>
                    <div className="flex items-center gap-4">
                        <Link
                            href="/"
                            className="text-sm text-muted-foreground hover:text-primary transition-colors"
                        >
                            Back to home
                        </Link>
                    </div>
                </div>
            </header>

            {/* Main content */}
            <div className="container mx-auto px-4 py-8">
                <div className="flex flex-col lg:flex-row gap-8 items-center justify-between">
                    {/* Right side - Auth form */}
                    <div className="w-full lg:w-2/3 max-w-xl">{children}</div>

                    {/* Left side - Marketing content */}
                    <div className="w-full lg:w-1/3 space-y-8">
                        {/* Feature cards - similar to landing page */}
                        <div className="flex items-center justify-center">
                            <div className="relative w-full max-w-[500px] aspect-video rounded-xl bg-muted p-4 shadow-lg">
                                <div className="grid grid-cols-2 gap-4 h-full">
                                    <div className="flex flex-col gap-2">
                                        <div className="h-1/2 rounded-lg bg-background p-3 flex flex-col">
                                            <div className="w-1/2 h-2 bg-primary/20 rounded mb-2"></div>
                                            <div className="flex-1 flex items-center justify-center">
                                                <BadgeDollarSign className="h-8 w-8 text-primary/60" />
                                            </div>
                                        </div>
                                        <div className="h-1/2 rounded-lg bg-background p-3 flex flex-col">
                                            <div className="w-1/3 h-2 bg-primary/20 rounded mb-2"></div>
                                            <div className="flex-1 flex items-center justify-center">
                                                <CreditCard className="h-8 w-8 text-primary/60" />
                                            </div>
                                        </div>
                                    </div>
                                    <div className="flex flex-col gap-2">
                                        <div className="h-2/3 rounded-lg bg-background p-3 flex flex-col">
                                            <div className="w-2/3 h-2 bg-primary/20 rounded mb-2"></div>
                                            <div className="flex-1 flex items-center justify-center">
                                                <BarChart3 className="h-8 w-8 text-primary/60" />
                                            </div>
                                        </div>
                                        <div className="h-1/3 rounded-lg bg-background p-3 flex flex-col">
                                            <div className="w-1/4 h-2 bg-primary/20 rounded mb-2"></div>
                                            <div className="flex-1 flex items-center justify-center">
                                                <HandCoins className="h-6 w-6 text-primary/60" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div className="space-y-4">
                            <h1 className="text-2xl font-bold tracking-tight">
                                Manage your finances with ease
                            </h1>
                            <p className="text-xl text-muted-foreground">
                                Join the users who trust Finance Tracker to
                                manage their personal and business finances.
                            </p>
                        </div>

                        <div className="grid grid-cols-1 gap-4">
                            <div className="bg-white p-6 rounded-lg border shadow-sm">
                                <div className="h-12 w-12 rounded-lg bg-primary/10 flex items-center justify-center mb-4">
                                    <BarChart3 className="h-6 w-6 text-primary" />
                                </div>
                                <h3 className="font-medium text-lg">
                                    Expense Tracking
                                </h3>
                                <p className="text-muted-foreground mt-2">
                                    Monitor your spending habits with detailed
                                    analytics
                                </p>
                            </div>

                            <div className="bg-white p-6 rounded-lg border shadow-sm">
                                <div className="h-12 w-12 rounded-lg bg-primary/10 flex items-center justify-center mb-4">
                                    <CreditCard className="h-6 w-6 text-primary" />
                                </div>
                                <h3 className="font-medium text-lg">
                                    Multiple Wallets
                                </h3>
                                <p className="text-muted-foreground mt-2">
                                    Manage different accounts in one place
                                </p>
                            </div>

                            <div className="bg-white p-6 rounded-lg border shadow-sm">
                                <div className="h-12 w-12 rounded-lg bg-primary/10 flex items-center justify-center mb-4">
                                    <Activity className="h-6 w-6 text-primary" />
                                </div>
                                <h3 className="font-medium text-lg">
                                    Loan Management
                                </h3>
                                <p className="text-muted-foreground mt-2">
                                    Keep track of borrowed and lent money
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
