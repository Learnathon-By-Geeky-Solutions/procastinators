import { Button } from "@/components/ui/button";
import {
    Card,
    CardContent,
    CardDescription,
    CardHeader,
    CardTitle,
} from "@/components/ui/card";
import { PlusIcon } from "lucide-react";
import CategoryTable from "@/components/category/category-table";
import { fetchCategories } from "@/lib/data/categories-data";
import { AddCategoryDialog } from "@/components/category/add-category-dialog";

export default async function CategoriesPage() {
    const categories = await fetchCategories();
    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            <div className="flex items-center justify-between">
                <div>
                    <p className="text-muted-foreground">
                        Manage categories for your personal transactions.
                    </p>
                </div>
                <div className="flex gap-2">
                    <AddCategoryDialog />
                </div>
            </div>

            <Card>
                <CardHeader>
                    <CardTitle>Personal Categories</CardTitle>
                    <CardDescription>
                        List of available categories for your personal
                        transactions.
                    </CardDescription>
                </CardHeader>
                <CardContent>
                    <CategoryTable categories={categories} />
                </CardContent>
            </Card>
        </div>
    );
}
