"use client";

import { usePathname } from "next/navigation";
import {
    Breadcrumb,
    BreadcrumbItem,
    BreadcrumbLink,
    BreadcrumbList,
    BreadcrumbPage,
    BreadcrumbSeparator,
} from "@/components/ui/breadcrumb";
import React from "react";
import Link from "next/link";

export default function AppBreadcrumbs() {
    const pathname = usePathname();
    const segments = pathname.split("/").filter((segment) => segment);
    return (
        <Breadcrumb>
            <BreadcrumbList>
                {segments.length === 0 ? (
                    <BreadcrumbItem>
                        <BreadcrumbPage>Root</BreadcrumbPage>
                    </BreadcrumbItem>
                ) : (
                    segments.map((segment, index) => {
                        const href =
                            "/" + segments.slice(0, index + 1).join("/");
                        const formattedSegment =
                            segment.charAt(0).toUpperCase() + segment.slice(1);
                        const isLastSegment = index === segments.length - 1;

                        return (
                            <React.Fragment key={href}>
                                {index > 0 && <BreadcrumbSeparator />}
                                <BreadcrumbItem key={href}>
                                    {isLastSegment ? (
                                        <BreadcrumbPage>
                                            {formattedSegment}
                                        </BreadcrumbPage>
                                    ) : (
                                        <BreadcrumbLink asChild>
                                            <Link href={href}>
                                                {formattedSegment}
                                            </Link>
                                        </BreadcrumbLink>
                                    )}
                                </BreadcrumbItem>
                            </React.Fragment>
                        );
                    })
                )}
            </BreadcrumbList>
        </Breadcrumb>
    );
}
