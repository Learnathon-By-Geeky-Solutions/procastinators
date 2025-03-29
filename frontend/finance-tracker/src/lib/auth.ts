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

export const config = {
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

                return await res.json();
            },
        }),
    ],
    pages: {
        signIn: "/auth/login",
    },
};

export const { handlers, signIn, signOut, auth } = NextAuth(config);
