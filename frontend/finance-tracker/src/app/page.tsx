import { auth } from "@/lib/auth";

export default async function Home() {
    const session = await auth();
    return (
        <div>
            <div>Home Page</div>
            <div>Logged in as: {session?.user?.email}</div>
        </div>
    );
}
