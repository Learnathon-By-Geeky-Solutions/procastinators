"use client";

import type React from "react";
import { usePathname } from "next/navigation";
import { ActivityIcon } from "lucide-react";
import {
    Sidebar,
    SidebarContent,
    SidebarHeader,
    SidebarInset,
    SidebarMenu,
    SidebarMenuButton,
    SidebarMenuItem,
    SidebarProvider,
    SidebarTrigger,
} from "@/components/ui/sidebar";
import { Separator } from "@/components/ui/separator";
import Link from "next/link";
import AppBreadcrumbs from "@/components/navigation/app-breadcrumb";
import NavUser from "./nav-user";

export function AppSidebar({
    children,
    routes,
}: {
    children: React.ReactNode;
    routes: Array<{
        title: string;
        icon: React.ReactNode;
        href: string;
    }>;
}) {
    const pathname = usePathname();

    return (
        <SidebarProvider>
            <div className="flex min-h-screen w-full">
                <Sidebar collapsible="icon">
                    {/* Sidebar header */}
                    <SidebarHeader>
                        <SidebarMenu>
                            <SidebarMenuItem>
                                <SidebarMenuButton size="lg">
                                    <span>
                                        <ActivityIcon size={24} />
                                    </span>
                                    <span className="font-bold text-xl">
                                        Finance Tracker
                                    </span>
                                </SidebarMenuButton>
                            </SidebarMenuItem>
                        </SidebarMenu>
                    </SidebarHeader>

                    {/* Sidebar navigation routes */}
                    <SidebarContent>
                        <SidebarMenu>
                            {routes.map((route) => {
                                return (
                                    <SidebarMenuItem key={route.href}>
                                        <SidebarMenuButton
                                            asChild
                                            isActive={pathname === route.href}
                                            tooltip={route.title}
                                        >
                                            <Link href={route.href}>
                                                {route.icon}
                                                <span>{route.title}</span>
                                            </Link>
                                        </SidebarMenuButton>
                                    </SidebarMenuItem>
                                );
                            })}
                        </SidebarMenu>
                    </SidebarContent>
                </Sidebar>

                {/* Main content */}
                <SidebarInset className="flex flex-col">
                    <header className="sticky top-0 z-10 flex h-16 items-center gap-4 border-b bg-background px-6">
                        <SidebarTrigger />
                        <Separator orientation="vertical" className="h-6" />
                        <div className="flex-1">
                            <AppBreadcrumbs />
                        </div>
                        <div>
                            <NavUser />
                        </div>
                    </header>
                    <main className="flex-1">{children}</main>
                </SidebarInset>
            </div>
        </SidebarProvider>
    );
}
