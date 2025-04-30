import { NextRequest, NextResponse } from "next/server";
import { auth, signIn } from "@/lib/auth";

export async function middleware(request: NextRequest) {
    const session = await auth();
    const { pathname } = request.nextUrl;

    if (!session) {
        if (pathname === "/" || pathname.startsWith("/auth")) {
            return NextResponse.next();
        }
        return NextResponse.redirect(new URL("/auth/login", request.url));
    }

    if (session.refreshAt && session.refreshAt < Date.now()) {
        try {
            await signIn("refresh", {
                redirect: false,
                refreshToken: session.refreshToken,
            });
        } catch (error) {
            console.error(error);
        }
    }

    if (pathname.startsWith("/auth")) {
        return NextResponse.redirect(new URL("/dashboard", request.url));
    }

    return NextResponse.next();
}

export const config = {
    matcher: [
        /*
         * Match all request paths except for the ones starting with:
         * - api (API routes)
         * - _next/static (static files)
         * - _next/image (image optimization files)
         * - favicon.ico, sitemap.xml, robots.txt (metadata files)
         */
        "/((?!api|_next/static|_next/image|favicon.ico|sitemap.xml|robots.txt).*)",
    ],
};
