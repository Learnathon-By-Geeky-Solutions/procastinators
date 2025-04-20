import { AppSidebar } from "@/components/navigation/app-sidebar";
import { CreditCard, LayoutDashboardIcon, Tags, Wallet } from "lucide-react";

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
    ];
    return <AppSidebar routes={routes}>{children}</AppSidebar>;
}
