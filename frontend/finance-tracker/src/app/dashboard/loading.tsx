import { Skeleton } from "@/components/ui/skeleton";
import {
    Card,
    CardContent,
    CardFooter,
    CardHeader,
} from "@/components/ui/card";

export default function DashboardSkeleton() {
    return (
        <div className="flex flex-col gap-4 p-4 md:p-8">
            {/* Header */}
            <div className="flex items-center justify-between">
                <div>
                    <Skeleton className="h-5 w-64" />
                </div>
            </div>

            {/* Cards Grid */}
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-4">
                {/* Total Balance Card */}
                <Card>
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <Skeleton className="h-4 w-24" />
                    </CardHeader>
                    <CardContent>
                        <Skeleton className="h-8 w-32 mb-2" />
                        <div className="flex gap-2 items-center">
                            <Skeleton className="h-4 w-32" />
                        </div>
                    </CardContent>
                </Card>

                {/* Total Income Card */}
                <Card>
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <Skeleton className="h-4 w-24" />
                    </CardHeader>
                    <CardContent>
                        <Skeleton className="h-8 w-32 mb-2" />
                        <Skeleton className="h-4 w-20" />
                    </CardContent>
                </Card>

                {/* Total Expenses Card */}
                <Card>
                    <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
                        <Skeleton className="h-4 w-24" />
                    </CardHeader>
                    <CardContent>
                        <Skeleton className="h-8 w-32 mb-2" />
                        <Skeleton className="h-4 w-20" />
                    </CardContent>
                </Card>
            </div>

            {/* Lower Section */}
            <div className="grid gap-4 md:grid-cols-2 lg:grid-cols-8 mt-4">
                {/* Recent Transactions */}
                <Card className="col-span-4">
                    <CardHeader>
                        <div className="flex items-center justify-between space-y-0">
                            <Skeleton className="h-6 w-48" />
                            <Skeleton className="h-8 w-8 rounded-full" />
                        </div>
                    </CardHeader>
                    <CardContent>
                        {/* Transaction Items */}
                        {[...Array(5)].map((_, index) => (
                            <div
                                key={index}
                                className="flex items-center justify-between py-2"
                            >
                                <div className="flex items-center gap-3">
                                    <Skeleton className="h-10 w-10 rounded-full" />
                                    <div className="space-y-1">
                                        <Skeleton className="h-4 w-32" />
                                        <Skeleton className="h-3 w-24" />
                                    </div>
                                </div>
                                <Skeleton className="h-4 w-20" />
                            </div>
                        ))}
                    </CardContent>
                    <CardFooter>
                        <div className="flex items-center justify-end w-full">
                            <Skeleton className="h-4 w-16" />
                        </div>
                    </CardFooter>
                </Card>

                {/* Expense Summary */}
                <Card className="col-span-4">
                    <CardHeader>
                        <Skeleton className="h-6 w-40 mb-2" />
                        <Skeleton className="h-4 w-56" />
                    </CardHeader>
                    <CardContent>
                        {/* Expense Breakdown Chart Skeleton */}
                        <div className="flex flex-col gap-4">
                            {/* Chart Placeholder */}
                            <div className="relative h-48 w-full">
                                <Skeleton className="h-full w-full rounded-lg" />
                            </div>

                            {/* Categories Placeholder */}
                            <div className="space-y-2">
                                {[...Array(4)].map((_, index) => (
                                    <div
                                        key={index}
                                        className="flex items-center justify-between"
                                    >
                                        <div className="flex items-center gap-2">
                                            <Skeleton className="h-3 w-3 rounded-full" />
                                            <Skeleton className="h-4 w-24" />
                                        </div>
                                        <Skeleton className="h-4 w-16" />
                                    </div>
                                ))}
                            </div>
                        </div>
                    </CardContent>
                </Card>
            </div>
        </div>
    );
}
