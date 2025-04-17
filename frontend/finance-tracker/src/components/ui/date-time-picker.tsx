"use client";

import * as React from "react";
import { format } from "date-fns";
import { Calendar as CalendarIcon } from "lucide-react";
import { cn } from "@/lib/utils";
import { Button } from "@/components/ui/button";
import { Calendar } from "@/components/ui/calendar";
import {
    Popover,
    PopoverContent,
    PopoverTrigger,
} from "@/components/ui/popover";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

interface DateTimePickerProps {
    date: Date | undefined;
    setDate: (date: Date | undefined) => void;
    className?: string;
    modal?: boolean;
}

export function DateTimePicker({
    date,
    setDate,
    className,
    modal = false,
}: DateTimePickerProps) {
    const [selectedTime, setSelectedTime] = React.useState(
        date ? format(date, "HH:mm") : ""
    );

    const handleTimeChange = (time: string) => {
        setSelectedTime(time);
        if (date && time) {
            const [hours, minutes] = time.split(":");
            const newDate = new Date(date);
            newDate.setHours(parseInt(hours, 10));
            newDate.setMinutes(parseInt(minutes, 10));
            setDate(newDate);
        }
    };

    return (
        <div className={cn("grid gap-2", className)}>
            <Popover modal={modal}>
                <PopoverTrigger asChild>
                    <Button
                        variant={"outline"}
                        className={cn(
                            "w-full justify-start text-left font-normal",
                            !date && "text-muted-foreground"
                        )}
                    >
                        <CalendarIcon className="mr-2 h-4 w-4" />
                        {date ? (
                            format(date, "PPP p")
                        ) : (
                            <span>Pick a date and time</span>
                        )}
                    </Button>
                </PopoverTrigger>
                <PopoverContent className="w-auto p-0" align="start">
                    <Calendar
                        mode="single"
                        selected={date}
                        onSelect={(selectedDate) => {
                            if (selectedDate) {
                                const [hours, minutes] = selectedTime
                                    ? selectedTime.split(":")
                                    : ["0", "0"];
                                selectedDate.setHours(parseInt(hours, 10));
                                selectedDate.setMinutes(parseInt(minutes, 10));
                            }
                            setDate(selectedDate);
                        }}
                        initialFocus
                    />
                    <div className="p-3 border-t">
                        <Label htmlFor="time">Time</Label>
                        <Input
                            id="time"
                            type="time"
                            value={selectedTime}
                            onChange={(e) => handleTimeChange(e.target.value)}
                            className="mt-2"
                        />
                    </div>
                </PopoverContent>
            </Popover>
        </div>
    );
}
