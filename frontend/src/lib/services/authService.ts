

export interface RegisterPayload {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
}

export interface ApiResponse<T> {
  success: boolean;
  data?: T;
  message?: string;
}

export async function registerUser(payload: RegisterPayload): Promise<ApiResponse<null>> {
  try {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL;

    if (!apiUrl) {
      throw new Error("NEXT_PUBLIC_API_URL is not defined. Check your .env.local file.");
    }

    const res = await fetch(`${apiUrl}/api/auth/register`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(payload),
    });

    if (!res.ok) {
      const data = await res.json().catch(() => ({}));
      throw new Error(data.message || "Registration failed");
    }

    return { success: true, data: null };
  } catch (err: any) {
    return { success: false, message: err.message };
  }
}

export async function loginUser(email: string, password: string) {
  try {
    const apiUrl = process.env.NEXT_PUBLIC_API_URL;
    if (!apiUrl) throw new Error("NEXT_PUBLIC_API_URL is not defined.");

    const res = await fetch(`${apiUrl}/api/auth/login`, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ email, password }),
    });

    if (!res.ok) {
      const data = await res.json().catch(() => ({}));
      throw new Error(data.message || "Login failed");
    }

    const data = await res.json();

    if (data.token) {
      localStorage.setItem("jwt_token", data.token);
    } else {
      throw new Error("No token returned from server");
    }

    return { success: true };
  } catch (err: any) {
    return { success: false, message: err.message };
  }
}

