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
import { DeleteWalletDialog } from "@/components/wallet/delete-wallet-dialog";

export default function ManageWalletDropdown({
    wallet,
}: {
    readonly wallet: Wallet;
}) {
    const [dropdownOpen, setDropdownOpen] = useState(false);
    const [editDialogOpen, setEditDialogOpen] = useState(false);
    const [deleteDialogOpen, setDeleteDialogOpen] = useState(false);

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
                    <DropdownMenuItem
                        className="text-destructive focus:text-destructive"
                        onClick={() => {
                            setDropdownOpen(false);
                            setDeleteDialogOpen(true);
                        }}
                    >
                        <Trash2Icon className="mr-2 h-4 w-4 text-inherit" />
                        Delete
                    </DropdownMenuItem>
                </DropdownMenuContent>
            </DropdownMenu>
            <EditWalletDialog
                wallet={wallet}
                open={editDialogOpen}
                setOpen={setEditDialogOpen}
            />
            <DeleteWalletDialog
                wallet={wallet}
                open={deleteDialogOpen}
                setOpen={setDeleteDialogOpen}
            />
        </>
    );
}
