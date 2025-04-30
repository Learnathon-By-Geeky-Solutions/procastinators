"use client";

import * as React from "react";
import * as ProgressPrimitive from "@radix-ui/react-progress";

import { cn } from "@/lib/utils";

const Progress = React.forwardRef<
    React.ElementRef<typeof ProgressPrimitive.Root>,
    React.ComponentPropsWithoutRef<typeof ProgressPrimitive.Root> & {
        // Add optional prop to disable color spectrum
        useColorSpectrum?: boolean;
    }
>(({ className, value, useColorSpectrum = true, ...props }, ref) => {
    // Convert undefined value to 0
    const safeValue = value || 0;

    // Style with CSS variables for the color spectrum
    const indicatorStyle = {
        transform: `translateX(-${100 - safeValue}%)`,
        // Only apply special background if color spectrum is enabled
        ...(useColorSpectrum
            ? {
                  background: `hsl(${safeValue * 1.2}, 100%, 35%)`,
                  // This creates a spectrum from red (0°) through yellow (~60°) to green (~120°)
                  // The multiplier 1.2 helps reach green (120°) when value is 100
                  // Using 35% lightness for darker shades
              }
            : {
                  // Use the default primary color when spectrum is disabled
                  backgroundColor: "var(--primary)",
              }),
    };

    return (
        <ProgressPrimitive.Root
            ref={ref}
            className={cn(
                "relative h-2 w-full overflow-hidden rounded-full bg-secondary/20",
                className
            )}
            {...props}
        >
            <ProgressPrimitive.Indicator
                className={cn(
                    "h-full w-full flex-1 transition-all",
                    // Only apply base class if not using spectrum
                    !useColorSpectrum && "bg-primary"
                )}
                style={indicatorStyle}
            />
        </ProgressPrimitive.Root>
    );
});

Progress.displayName = ProgressPrimitive.Root.displayName;

export { Progress };
