export async function attemptLogin(email: string, password: string) {
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
}

export async function fetchUserInfo(token: string) {
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
            return {
                name: data?.email.split("@")[0],
                email: data?.email,
            };
        }
    } catch (error) {
        console.error(error);
    }
}

export async function fetchAccessToken(refreshToken: string) {
    try {
        const url = `${process.env.BACKEND_BASE_URL}/identity/refresh`;
        console.log("Fetching refresh token from", url);
        return await fetch(url, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ refreshToken }),
        });
    } catch (error) {
        console.error(error);
    }
}
