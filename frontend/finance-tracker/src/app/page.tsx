import { auth } from "@/lib/auth";

export default async function Home() {
    const session = await auth();
    console.log(session);
    return <>Home Page</>;
}
