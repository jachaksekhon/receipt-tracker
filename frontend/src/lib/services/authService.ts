/* eslint-disable @typescript-eslint/no-explicit-any */
import { apiFetch } from "./api_client";

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
    await apiFetch("/api/auth/register", {
      method: "POST",
      body: JSON.stringify(payload),
    });
    return { success: true, data: null };
  } catch (err: any) {
    return { success: false, message: err.message };
  }
}

export async function loginUser(email: string, password: string): Promise<ApiResponse<{ token: string }>> {
  try {
    const data = await apiFetch<{ token: string }>("/api/auth/login", {
      method: "POST",
      body: JSON.stringify({ email, password }),
    });

    if (data.token) {
      localStorage.setItem("token", data.token);
      return { success: true, data };
    } else {
      throw new Error("No token returned from server");
    }
  } catch (err: any) {
    return { success: false, message: err.message };
  }
}

export function logoutUser(): void {
  localStorage.removeItem("token");
}
