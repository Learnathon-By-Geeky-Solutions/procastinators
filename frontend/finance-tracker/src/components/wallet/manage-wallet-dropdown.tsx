"use client";

import {
    DropdownMenu,
    DropdownMenuContent,
    DropdownMenuItem,
    DropdownMenuTrigger,
} from "@/components/ui/dropdown-menu";
import { Button } from "@/components/ui/button";
import { MoreHorizontalIcon, PencilIcon, Trash2Icon } from "lucide-react";
import { EditWalletDialog } from "@/components/wallet/edit-wallet-dialog";
import { useState } from "react";
import { Wallet } from "@/lib/definitions";

export default function ManageWalletDropdown({ wallet }: { wallet: Wallet }) {
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    return (
        <>
            <DropdownMenu open={dropdownOpen} onOpenChange={setDropdownOpen}>
                <DropdownMenuTrigger asChild>
                    <Button variant="ghost" size="icon" className="h-8 w-8">
                        <MoreHorizontalIcon className="h-4 w-4" />
                        <span className="sr-only">Open menu</span>
                    </Button>
                </DropdownMenuTrigger>
                <DropdownMenuContent align="end">
                    <DropdownMenuItem
                        onClick={() => {
                            setDropdownOpen(false);
                            setEditDialogOpen(true);
                        }}
                    >
                        <PencilIcon className="mr-2 h-4 w-4" />
                        Edit
                    </DropdownMenuItem>
                    <DropdownMenuItem className="text-destructive focus:text-destructive">
                        <Trash2Icon className="mr-2 h-4 w-4" />
                        Delete
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
            <EditWalletDialog
                wallet={wallet}
                open={editDialogOpen}
                setOpen={setEditDialogOpen}
            />
        </>
    );
}
