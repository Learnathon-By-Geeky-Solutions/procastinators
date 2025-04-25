import NextAuth from "next-auth";
import Credentials from "next-auth/providers/credentials";
import {
    attemptLogin,
    fetchAccessToken,
    fetchUserInfo,
} from "@/lib/data/auth-data";

export const { handlers, signIn, signOut, auth } = NextAuth({
    providers: [
        Credentials({
            credentials: {
                email: {},
                password: {},
            },
            authorize: async (credentials) => {
                const { email, password } = credentials as {
                    email: string;
                    password: string;
                };
                const res = await attemptLogin(email, password);

                if (!res?.ok) {
                    console.error(res);
                    return null;
                }

                const token = await res.json();
                const user = await fetchUserInfo(token.accessToken);
                return { ...user, token };
            },
        }),
    ],

    pages: {
        signIn: "/auth/login",
    },

    session: {
        strategy: "jwt",
        maxAge: 3600,
    },

    callbacks: {
        jwt: async ({ token, user }) => {
            if (user) {
                token.accessToken = user.token?.accessToken;
                token.refreshToken = user.token?.refreshToken;
                return token;
            } else if (token.exp && token.exp * 1000 > Date.now()) {
                return token;
            } else if (token.refreshToken) {
                const res = await fetchAccessToken(token.refreshToken);
                if (!res?.ok) {
                    console.error(res);
                    return token;
                }
                const newToken = await res.json();
                token.accessToken = newToken.accessToken;
                token.refreshToken = newToken.refreshToken;
                token.exp = Math.floor(Date.now() / 1000) + 3600;
                return token;
            }

            return token;
        },
        session: async ({ session, token }) => {
            if (token) {
                session.accessToken = token.accessToken;
                session.refreshToken = token.refreshToken;
            }
            return session;
        },
    },
});
