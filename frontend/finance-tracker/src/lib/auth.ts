import { sign } from "crypto";
import NextAuth from "next-auth";
import Credentials from "next-auth/providers/credentials";

export const config = {
    providers: [
        Credentials({
            credentials: {
                email: {},
                password: {},
            },
            authorize: async (credentials) => {
                console.log("Credentials: ", credentials);
                if (
                    credentials.email === "admin@test.com" &&
                    credentials.password === "admin"
                ) {
                    return {
                        name: "Admin",
                    };
                }

                return null;
            },
        }),
    ],
    pages: {
        signIn: "/auth/login",
    },
};

export const { handlers, signIn, signOut, auth } = NextAuth(config);
