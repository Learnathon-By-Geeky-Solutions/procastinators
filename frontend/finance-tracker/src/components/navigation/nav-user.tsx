"use client";

import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuSeparator,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Avatar, AvatarFallback, AvatarImage } from "@/components/ui/avatar";
import { Button } from "@/components/ui/button";
import { ContactIcon, HelpCircle, LogOut, Settings } from "lucide-react";
import Link from "next/link";
import { signOut, useSession } from "next-auth/react";

export default function NavUser() {
    const { data: session } = useSession();
    return (
        <DropdownMenu>
            {/* user info */}
            <DropdownMenuTrigger asChild>
                <div className="flex items-center justify-between p-2 cursor-pointer hover:bg-sidebar-accent rounded-md transition-colors">
                    <div className="flex items-center">
                        <Avatar>
                            <AvatarImage src="/placeholder.svg?height=40&width=40" />
                            <AvatarFallback className="border">
                                {session?.user?.name![0].toUpperCase()}
                            </AvatarFallback>
                        </Avatar>
                        <div className="ml-2 space-y-1 hidden group-data-[state=expanded]:block">
                            <p className="text-sm font-medium leading-none">
                                {session?.user?.name}
                            </p>
                            <p className="text-xs leading-none text-muted-foreground">
                                {session?.user?.email}
                            </p>
                        </div>
                    </div>
                </div>
            </DropdownMenuTrigger>

            <DropdownMenuContent align="end" className="w-56">
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
