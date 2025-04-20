"use client";
import { TotalPerCategory } from "@/lib/definitions";
import {
    PieChart,
    Pie,
    Cell,
    ResponsiveContainer,
    Legend,
    Tooltip,
} from "recharts";

const COLOR_PALETTE = [
    "#FF3E6C", // hot pink
    "#FF8042", // orange
    "#00B3E6", // sky blue
    "#3CB44B", // bright green
    "#9370DB", // medium purple
    "#0088FE", // vivid blue
    "#FFBB28", // golden yellow
    "#00C49F", // teal
    "#E6194B", // crimson
    "#911EB4", // purple
    "#4363D8", // royal blue
    "#42D4F4", // cyan
    "#F032E6", // magenta
    "#FABEBE", // salmon
    "#469990", // teal green
    "#E6BEFF", // lavender
    "#9A6324", // brown
    "#800000", // maroon
    "#FFD700", // gold
];

export function ExpenseBreakdown({
    report,
}: {
    readonly report: TotalPerCategory[];
}) {
    const chartData = report.map((item, index) => ({
        name: item.categoryTitle,
        value: item.total,
        color: COLOR_PALETTE[index % COLOR_PALETTE.length],
    }));

    return (
        <div className="h-[300px] w-full">
            <ResponsiveContainer width="100%" height="100%">
                <PieChart>
                    <Pie
                        data={chartData}
                        cx="50%"
                        cy="50%"
                        innerRadius={60}
                        outerRadius={80}
                        paddingAngle={5}
                        dataKey="value"
                        label={({ name, percent }) =>
                            `${name} ${(percent * 100).toFixed(0)}%`
                        }
                        labelLine={false}
                    >
                        {chartData.map((entry, index) => (
                            <Cell key={`cell-${index}`} fill={entry.color} />
                        ))}
                    </Pie>
                    <Tooltip formatter={(value) => [`${value} BDT`, "Total"]} />
                    <Legend />
                </PieChart>
            </ResponsiveContainer>
        </div>
    );
}
