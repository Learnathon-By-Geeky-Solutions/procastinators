import { AppSidebar } from "@/components/navigation/app-sidebar";
import { auth } from "@/lib/auth";
import {
    ArrowRightLeftIcon,
    BanknoteArrowDownIcon,
    ClipboardPenLineIcon,
    CreditCard,
    LayoutDashboardIcon,
    Tags,
    Wallet,
} from "lucide-react";

export default async function DashboardLayout({
    children,
}: Readonly<{
    children: React.ReactNode;
}>) {
    const routes = [
        {
            title: "Overview",
            icon: <LayoutDashboardIcon className="h-5 w-5" />,
            href: "/dashboard",
        },
        {
            title: "Wallets",
            icon: <Wallet className="h-5 w-5" />,
            href: "/dashboard/wallets",
        },
        {
            title: "Categories",
            icon: <Tags className="h-5 w-5" />,
            href: "/dashboard/categories",
        },
        {
            title: "Transactions",
            icon: <CreditCard className="h-5 w-5" />,
            href: "/dashboard/transactions",
        },
        {
            title: "Loans",
            icon: <ArrowRightLeftIcon className="h-5 w-5" />,
            href: "/dashboard/loans",
        },
        {
            title: "Loan Requests",
            icon: <ClipboardPenLineIcon className="h-5 w-5" />,
            href: "/dashboard/loan-requests",
        },
        {
            title: "Claim Funds",
            icon: <BanknoteArrowDownIcon className="h-5 w-5" />,
            href: "/dashboard/claimables",
        },
    ];
    const session = await auth();
    return (
        <AppSidebar routes={routes} session={session}>
            {children}
        </AppSidebar>
    );
}
