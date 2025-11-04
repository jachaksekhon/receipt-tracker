export function getAuthToken(): string | null {
  return typeof window !== "undefined"
    ? localStorage.getItem("jwt_token")
    : null;
}

export function clearAuthToken() {
  if (typeof window !== "undefined") {
    localStorage.removeItem("jwt_token");
  }
}
