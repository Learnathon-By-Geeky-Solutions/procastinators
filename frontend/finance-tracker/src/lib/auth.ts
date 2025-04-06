import NextAuth from "next-auth";
import Credentials from "next-auth/providers/credentials";

const attemptLogin = async (email: string, password: string) => {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/identity/login`;
        console.log("Attempting login to", url);
        return await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ email, password }),
        });
    } catch (error) {
        console.error(error);
    }
};

const fetchUserInfo = async (token: string) => {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/identity/manage/info`;
        console.log("Fetching user info from", url);
        const res = await fetch(url, {
            method: "GET",
            headers: {
                Authorization: `Bearer ${token}`,
            },
        });
        if (res?.ok) {
            const data = await res.json();
            return await {
                name: data?.email.split("@")[0] ?? "Not Found",
                email: data?.email ?? "Not Found",
            };
        }
    } catch (error) {
        console.error(error);
    }
};

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
            console.log("JWT:", token, user);
            if (user) {
                token.accessToken = user.token?.accessToken;
                token.refreshToken = user.token?.refreshToken;
            }
            return token;
        },
        session: async ({ session, token }) => {
            console.log("Session:", session, token);
            if (token) {
                session.accessToken = token.accessToken;
                session.refreshToken = token.refreshToken;
            }
            return session;
        },
    },
});
