import { Navigate, useLocation } from "react-router-dom";
import { useAuth } from "./useAuth";
import type React from "react";

export default function RequireAuth({ children }: { children: React.ReactNode }) {
    const { isAuthed } = useAuth();
    const location = useLocation();

    if (!isAuthed) {
        // Redirect them to login, and remember where they tried to go
        return <Navigate to="/login" replace state={{ from: location.pathname }} />;
    }

    // Otherwise, render the protected page
    return children;
}
