"use client";

import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuGroup,
    DropdownMenuItem,
    DropdownMenuLabel,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { ContactIcon, HelpCircle, LogOut, Settings } from "lucide-react";
import Link from "next/link";
import { signOut, useSession } from "next-auth/react";

function UserAvatar(fallbackText: string) {
    return (
        <Avatar>
            <AvatarImage src="/placeholder.svg?height=40&width=40" />
            <AvatarFallback className="border">{fallbackText}</AvatarFallback>
        </Avatar>
    );
}

export default function NavUser() {
    const { data: session } = useSession();
    const user = session?.user;
    const avatarFallbackText = (user?.name ?? "u").slice(0, 1).toUpperCase();

    return (
        <DropdownMenu>
            {/* user info */}
            <DropdownMenuTrigger asChild>
                <div className="flex items-center justify-between p-2 cursor-pointer hover:bg-sidebar-accent rounded-md transition-colors">
                    <div className="flex items-center">
                        {UserAvatar(avatarFallbackText)}
                    </div>
                </div>
            </DropdownMenuTrigger>

            <DropdownMenuContent align="end" className="w-56">
                <DropdownMenuLabel className="p-0 font-normal">
                    <div className="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
                        {UserAvatar(avatarFallbackText)}
                        <div className="grid flex-1 text-left text-sm leading-tight">
                            <span className="truncate font-semibold">
                                {user?.name}
                            </span>
                            <span className="truncate text-xs">
                                {user?.email}
                            </span>
                        </div>
                    </div>
                </DropdownMenuLabel>
                <DropdownMenuSeparator />
                <DropdownMenuGroup>
                    <DropdownMenuItem asChild>
                        <Link href={`/profile`}>
                            <ContactIcon className="mr-2 h-4 w-4" />
                            Profile
                        </Link>
                    </DropdownMenuItem>
                    <DropdownMenuItem>
                        <Settings className="mr-2 h-4 w-4" />
                        <span>Settings</span>
                    </DropdownMenuItem>
                    <DropdownMenuItem>
                        <HelpCircle className="mr-2 h-4 w-4" />
                        <span>Help & Support</span>
                    </DropdownMenuItem>
                </DropdownMenuGroup>

                <DropdownMenuSeparator />
                <DropdownMenuItem className="py-0 text-red-600">
                    <Button
                        className="w-full justify-start"
                        variant="ghost"
                        size="icon"
                        onClick={async () => await signOut()}
                    >
                        <LogOut className="h-4 w-4 text-inherit" />
                        <span>Log out</span>
                    </Button>
                </DropdownMenuItem>
            </DropdownMenuContent>
        </DropdownMenu>
    );
}
