import Link from "next/link";
import { Button } from "@/components/ui/button";
import {
    Activity,
    ArrowRight,
    BadgeDollarSign,
    BarChart3,
    CreditCard,
    HandCoins,
} from "lucide-react";
import { auth } from "@/lib/auth";

export default async function Home() {
    const session = await auth();
    return (
        <div className="flex flex-col min-h-screen">
            {/* Navigation */}
            <header className="border-b">
                <div className="container mx-auto flex h-16 items-center justify-between px-4 md:px-6">
                    <div className="flex items-center gap-2">
                        <Activity className="h-6 w-6 text-primary" />
                        <span className="text-xl font-bold">
                            Finance Tracker
                        </span>
                    </div>
                    <nav className="hidden md:flex gap-6">
                        <Link
                            href="#features"
                            className="text-sm font-medium hover:underline underline-offset-4"
                        >
                            Features
                        </Link>
                        <Link
                            href="#how-it-works"
                            className="text-sm font-medium hover:underline underline-offset-4"
                        >
                            How It Works
                        </Link>
                    </nav>
                    <div className="flex items-center gap-4">
                        {session ? (
                            <Link href="/dashboard">
                                <Button size="sm">Dashboard</Button>
                            </Link>
                        ) : (
                            <Link href="/auth/login">
                                <Button size="sm">Login</Button>
                            </Link>
                        )}
                    </div>
                </div>
            </header>

            {/* Hero Section */}
            <section className="w-full py-12 md:py-24 lg:py-32 bg-gradient-to-b from-background to-muted">
                <div className="container mx-auto px-4 md:px-6">
                    <div className="grid gap-6 lg:grid-cols-2 lg:gap-12 items-center">
                        <div className="flex flex-col justify-center space-y-4">
                            <div className="space-y-2">
                                <h1 className="text-3xl font-bold tracking-tighter sm:text-5xl xl:text-6xl/none">
                                    Simplify Finance, Ease Collaboration
                                </h1>
                                <p className="max-w-[600px] text-muted-foreground md:text-xl mt-6">
                                    Track expenses, manage loans, and
                                    collaborate with friends and family all in
                                    one place.
                                </p>
                            </div>
                            <div className="flex flex-col gap-2 min-[400px]:flex-row mt-4">
                                <Link href="/auth/register">
                                    <Button size="lg" className="gap-1.5">
                                        Get Started
                                        <ArrowRight className="h-4 w-4" />
                                    </Button>
                                </Link>
                            </div>
                        </div>
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
                    </div>
                </div>
            </section>

            {/* Features Section */}
            <section className="w-full py-12 md:py-24 lg:py-32" id="features">
                <div className="container mx-auto px-4 md:px-6">
                    <div className="flex flex-col items-center justify-center space-y-4 text-center">
                        <div className="space-y-2">
                            <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                                Key Features
                            </h2>
                            <p className="max-w-[900px] text-muted-foreground md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                                Everything you need to manage your finances in
                                one place
                            </p>
                        </div>
                    </div>
                    <div className="mx-auto grid max-w-5xl grid-cols-1 gap-6 py-12 md:grid-cols-2 lg:grid-cols-3">
                        <div className="flex flex-col items-center space-y-2 rounded-lg border p-6 shadow-sm">
                            <div className="rounded-full bg-primary/10 p-3">
                                <CreditCard className="h-6 w-6 text-primary" />
                            </div>
                            <h3 className="text-xl font-bold">
                                Wallet Management
                            </h3>
                            <p className="text-sm text-muted-foreground text-center">
                                Track multiple wallets and accounts in one place
                                with real-time balance updates.
                            </p>
                        </div>
                        <div className="flex flex-col items-center space-y-2 rounded-lg border p-6 shadow-sm">
                            <div className="rounded-full bg-primary/10 p-3">
                                <BarChart3 className="h-6 w-6 text-primary" />
                            </div>
                            <h3 className="text-xl font-bold">
                                Expense Tracking
                            </h3>
                            <p className="text-sm text-muted-foreground text-center">
                                Categorize and monitor your spending habits with
                                detailed analytics and reports.
                            </p>
                        </div>
                        <div className="flex flex-col items-center space-y-2 rounded-lg border p-6 shadow-sm">
                            <div className="rounded-full bg-primary/10 p-3">
                                <HandCoins className="h-6 w-6 text-primary" />
                            </div>
                            <h3 className="text-xl font-bold">
                                Loan Management
                            </h3>
                            <p className="text-sm text-muted-foreground text-center">
                                Track borrowed and lent money with installment
                                plans and payment reminders.
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            {/* How It Works Section */}
            <section
                className="w-full py-12 md:py-24 lg:py-32 bg-muted"
                id="how-it-works"
            >
                <div className="container mx-auto px-4 md:px-6">
                    <div className="flex flex-col items-center justify-center space-y-4 text-center">
                        <div className="space-y-2">
                            <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                                How It Works
                            </h2>
                            <p className="max-w-[900px] text-muted-foreground md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                                Get started with Finance Tracker in just a few
                                simple steps
                            </p>
                        </div>
                    </div>
                    <div className="mx-auto grid max-w-5xl grid-cols-1 gap-8 py-12 md:grid-cols-3">
                        <div className="flex flex-col items-center space-y-4 rounded-lg p-4">
                            <div className="flex h-16 w-16 items-center justify-center rounded-full bg-primary text-3xl font-bold text-primary-foreground">
                                1
                            </div>
                            <h3 className="text-xl font-bold">
                                Create Your Account
                            </h3>
                            <p className="text-sm text-muted-foreground text-center">
                                Sign up and set up your profile with your
                                financial preferences and goals.
                            </p>
                        </div>
                        <div className="flex flex-col items-center space-y-4 rounded-lg p-4">
                            <div className="flex h-16 w-16 items-center justify-center rounded-full bg-primary text-3xl font-bold text-primary-foreground">
                                2
                            </div>
                            <h3 className="text-xl font-bold">
                                Add Your Wallets
                            </h3>
                            <p className="text-sm text-muted-foreground text-center">
                                Connect your bank accounts or add manual wallets
                                to track your finances.
                            </p>
                        </div>
                        <div className="flex flex-col items-center space-y-4 rounded-lg p-4">
                            <div className="flex h-16 w-16 items-center justify-center rounded-full bg-primary text-3xl font-bold text-primary-foreground">
                                3
                            </div>
                            <h3 className="text-xl font-bold">
                                Start Tracking
                            </h3>
                            <p className="text-sm text-muted-foreground text-center">
                                Begin recording transactions, managing loans,
                                and tracking your financial progress.
                            </p>
                        </div>
                    </div>
                </div>
            </section>

            {/* CTA Section */}
            <section className="w-full py-12 md:py-24 lg:py-32 bg-primary text-primary-foreground">
                <div className="container mx-auto px-4 md:px-6">
                    <div className="flex flex-col items-center justify-center space-y-4 text-center">
                        <div className="space-y-2">
                            <h2 className="text-3xl font-bold tracking-tighter sm:text-4xl md:text-5xl">
                                Ready to Take Control of Your Finances?
                            </h2>
                            <p className="max-w-[900px] md:text-xl/relaxed lg:text-base/relaxed xl:text-xl/relaxed">
                                Join the users who have transformed their
                                financial management with Finance Tracker
                            </p>
                        </div>
                        <div className="flex flex-col gap-2 min-[400px]:flex-row">
                            <Link href="/auth/register">
                                <Button
                                    size="lg"
                                    variant="secondary"
                                    className="gap-1.5"
                                >
                                    Sign Up Now
                                    <ArrowRight className="h-4 w-4" />
                                </Button>
                            </Link>
                        </div>
                    </div>
                </div>
            </section>

            {/* Footer */}
            <footer className="border-t">
                <div className="container mx-auto flex flex-col gap-6 py-8 md:flex-row md:items-center md:justify-between px-4 md:px-6">
                    <div className="flex items-center gap-2">
                        <Activity className="h-6 w-6 text-primary" />
                        <span className="text-xl font-bold">
                            Finance Tracker
                        </span>
                    </div>
                    <p className="text-xs text-muted-foreground md:text-sm">
                        © {new Date().getFullYear()} Finance Tracker. All rights
                        reserved.
                    </p>
                    <div className="flex gap-4">
                        <Link
                            href="#"
                            className="text-sm font-medium hover:underline underline-offset-4"
                        >
                            Terms
                        </Link>
                        <Link
                            href="#"
                            className="text-sm font-medium hover:underline underline-offset-4"
                        >
                            Privacy
                        </Link>
                        <Link
                            href="#"
                            className="text-sm font-medium hover:underline underline-offset-4"
                        >
                            Contact
                        </Link>
                    </div>
                </div>
            </footer>
        </div>
    );
}
