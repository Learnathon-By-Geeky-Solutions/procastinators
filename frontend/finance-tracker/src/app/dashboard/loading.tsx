import { Skeleton } from "@/components/ui/skeleton";

export default function Loading() {
    return (
        <div className="flex flex-col gap-6 p-4 md:p-8">
            {/* Header Section */}
            <div className="flex items-center justify-between">
                <div className="flex flex-col gap-2">
                    <Skeleton className="h-6 w-48" />
                    <Skeleton className="h-4 w-64" />
                </div>
                <div className="flex items-center gap-2">
                    <Skeleton className="h-8 w-8 rounded-md" />
                    <Skeleton className="h-8 w-20 rounded-md" />
                </div>
            </div>

            {/* Summary Section */}
            <div className="grid gap-4 grid-cols-1 sm:grid-cols-2 lg:grid-cols-4">
                {[...Array(4)].map((_, index) => (
                    <div
                        key={index}
                        className={`${index >= 3 ? "hidden lg:block" : ""}`}
                    >
                        <div className="flex justify-between items-center mb-3">
                            <Skeleton className="h-4 w-24" />
                            <Skeleton className="h-6 w-6 rounded-full" />
                        </div>
                        <Skeleton className="h-8 w-full max-w-[120px] mb-3" />
                        <div className="flex items-center gap-2">
                            <Skeleton className="h-4 w-4 rounded-full" />
                            <Skeleton className="h-4 w-20" />
                        </div>
                    </div>
                ))}
            </div>

            {/* Main Content Area */}
            <div className="grid gap-6 grid-cols-1 lg:grid-cols-12">
                <div className="lg:col-span-5 space-y-6">
                    <div>
                        <Skeleton className="h-5 w-32 mb-3" />
                        <Skeleton className="h-4 w-full mb-3" />

                        <div className="aspect-video relative mb-3">
                            <Skeleton className="h-full w-full rounded-md" />
                        </div>

                        <div className="space-y-2">
                            {[...Array(3)].map((_, index) => (
                                <div
                                    key={index}
                                    className="flex items-center justify-between"
                                >
                                    <div className="flex items-center gap-2">
                                        <Skeleton className="h-3 w-3 rounded-full" />
                                        <Skeleton className="h-4 w-20" />
                                    </div>
                                    <Skeleton className="h-4 w-12" />
                                </div>
                            ))}
                        </div>
                    </div>

                    <div>
                        <Skeleton className="h-5 w-24 mb-3" />
                        <div className="space-y-2">
                            {[...Array(4)].map((_, index) => (
                                <div
                                    key={index}
                                    className="flex items-center justify-between"
                                >
                                    <Skeleton className="h-4 w-40" />
                                    <Skeleton className="h-4 w-16" />
                                </div>
                            ))}
                        </div>
                    </div>
                </div>

                <div className="lg:col-span-5 space-y-6">
                    <div>
                        <Skeleton className="h-5 w-32 mb-3" />
                        <Skeleton className="h-4 w-full mb-3" />

                        <div className="aspect-video relative mb-3">
                            <Skeleton className="h-full w-full rounded-md" />
                        </div>

                        <div className="space-y-2">
                            {[...Array(3)].map((_, index) => (
                                <div
                                    key={index}
                                    className="flex items-center justify-between"
                                >
                                    <div className="flex items-center gap-2">
                                        <Skeleton className="h-3 w-3 rounded-full" />
                                        <Skeleton className="h-4 w-20" />
                                    </div>
                                    <Skeleton className="h-4 w-12" />
                                </div>
                            ))}
                        </div>
                    </div>

                    <div>
                        <Skeleton className="h-5 w-24 mb-3" />
                        <div className="space-y-2">
                            {[...Array(4)].map((_, index) => (
                                <div
                                    key={index}
                                    className="flex items-center justify-between"
                                >
                                    <Skeleton className="h-4 w-40" />
                                    <Skeleton className="h-4 w-16" />
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    );
}
