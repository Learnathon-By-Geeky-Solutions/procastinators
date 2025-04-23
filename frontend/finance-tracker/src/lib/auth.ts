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
        Credentials({
            id: "refresh",
            credentials: {},
            authorize: async (credentials) => {
                const { refreshToken } = credentials as {
                    refreshToken: string;
                };
                const res = await fetchAccessToken(refreshToken);

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
        maxAge: 36000 * 24 * 7,
    },

    callbacks: {
        jwt: async ({ token, user }) => {
            if (user) {
                token.accessToken = user.token?.accessToken;
                token.refreshToken = user.token?.refreshToken;
                token.expiresIn = setRefreshAt(user.token?.expiresIn);
                return token;
            } else if (token.expiresIn && token.expiresIn > Date.now()) {
                const timeLeft = (token.expiresIn - Date.now()) / 1000;
                console.log(
                    `Expires in: ${Math.floor(timeLeft / 60)}m ${Math.floor(
                        timeLeft % 60
                    )}s`
                );
                return token;
            }

            return token;
        },
        session: async ({ session, token }) => {
            if (token) {
                session.accessToken = token.accessToken;
                session.refreshToken = token.refreshToken;
                session.refreshAt = token.expiresIn;
            }
            return session;
        },
    },
});

function setRefreshAt(expiresIn: number | undefined) {
    const expires = expiresIn ?? 3600;
    return Date.now() + expires * 1000 - (expires / 2) * 1000;
}
