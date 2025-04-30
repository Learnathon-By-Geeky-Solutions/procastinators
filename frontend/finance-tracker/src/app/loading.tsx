import { Activity } from "lucide-react";

export default function RootLoading() {
    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-background">
            <div className="flex flex-col items-center gap-6 text-center">
                {/* Brand Logo and Name */}
                <div className="flex items-center gap-2 text-primary animate-pulse">
                    <Activity size={40} />
                    <h1 className="text-3xl font-bold">Finance Tracker</h1>
                </div>

                {/* Brand Motto */}
                <p className="text-lg text-muted-foreground mb-8">
                    Simplify Finance, Ease Collaboration
                </p>

                {/* Loading Spinner */}
                <div className="h-12 w-12 rounded-full border-4 border-primary/30 border-t-primary animate-spin" />

                {/* Loading Text */}
                <p className="text-sm text-muted-foreground mt-4">Loading...</p>
            </div>
        </div>
    );
}
