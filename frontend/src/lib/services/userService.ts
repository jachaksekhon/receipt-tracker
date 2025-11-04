import { apiFetch } from "./api_client";

export interface UserProfile {
    firstName: string;
    lastName: string;
    email: string;
    createdAt: string;
}

export async function getCurrentUser(): Promise<UserProfile> {
  return apiFetch<UserProfile>("/api/user/me");
}
