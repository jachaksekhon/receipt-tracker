// src/services/user_service.ts
import { apiFetch } from "./api_client";

export interface UserProfile {
  id: number;
  email: string;
  createdAt: string;
}

export async function getCurrentUser(): Promise<UserProfile> {
  return apiFetch<UserProfile>("/api/users/me");
}
