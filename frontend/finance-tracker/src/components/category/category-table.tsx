import {
    Table,
    TableBody,
    TableCell,
    TableHead,
    TableHeader,
    TableRow,
} from "@/components/ui/table";
import { Badge } from "@/components/ui/badge";
import { Button } from "@/components/ui/button";
import { PencilIcon, Trash2Icon } from "lucide-react";
import { Category } from "@/lib/definitions";
import { EditCategoryDialog } from "@/components/category/edit-category-dialog";

export default function CategoryTable({
    categories,
}: {
    readonly categories: Category[];
}) {
    return (
        <Table>
            <TableHeader>
                <TableRow>
                    <TableHead>Title</TableHead>
                    <TableHead>Default Transaction Type</TableHead>
                    <TableHead className="text-right pr-5">Actions</TableHead>
                </TableRow>
            </TableHeader>
            <TableBody>
                {categories.map((category) => (
                    <TableRow key={category.id}>
                        <TableCell>
                            <span className="font-medium">
                                {category.title}
                            </span>
                        </TableCell>
                        <TableCell>
                            {category.defaultTransactionType === "Income" ? (
                                <Badge
                                    variant="outline"
                                    className="bg-green-100 text-green-800 hover:bg-green-100"
                                >
                                    Income
                                </Badge>
                            ) : (
                                <Badge
                                    variant="outline"
                                    className="bg-yellow-100 text-yellow-800 hover:bg-yellow-100"
                                >
                                    Expense
                                </Badge>
                            )}
                        </TableCell>
                        <TableCell className="text-right">
                            <EditCategoryDialog category={category} />
                            <Button variant="ghost" size="icon">
                                <Trash2Icon className="h-4 w-4 text-destructive" />
                            </Button>
                        </TableCell>
                    </TableRow>
                ))}
            </TableBody>
        </Table>
    );
}
