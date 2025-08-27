import { createContext, useContext, useEffect, useMemo, useState } from "react";
import { jwtDecode } from "jwt-decode"

type JwtPayload = {
    exp?: number;
    userId?: number;
};

export type AuthUser = {
    id: number;
    email: string;
    name: string | null;
};

// State for our app to check
type AuthState = {
    token: string | null;
    user: AuthUser | null;
    isAuthed: boolean;
}

// Extend AuthState with methods
// Need a way to clear/update token
type AuthContextValue = AuthState & {
    frontEndlogin: (token: string, user: AuthUser) => void;
    frontEndlogout: () => void;
};

const TOKEN_KEY = "JWT_TOKEN";
const USER_KEY = "JWT_USER";

// inits a global shared context
const AuthContext = createContext<AuthContextValue | null>(null);

// ---- Helpers ----
function isExpired(token: string): boolean {
    try {
        const { exp } = jwtDecode<JwtPayload>(token);

        if (!exp) return false;

        const now = Math.floor(Date.now() / 1000);
        return exp < now;

    } catch {
        return true;
    }
}

function parseStateFromStorage(): AuthState {
    const token = localStorage.getItem(TOKEN_KEY);

    if (!token || isExpired(token)) {
        return { token: null, user: null, isAuthed: false };
    }

    const userRaw = localStorage.getItem(USER_KEY);
    const user = userRaw ? (JSON.parse(userRaw) as AuthUser) : null;

    return { token, user, isAuthed: true };
}

// Main function

export function AuthProvider({ children } : { children: React.ReactNode }) {

    const[state, setState] = useState<AuthState>(() => parseStateFromStorage())

    useEffect(() => { 
        const onStorage = () => setState(parseStateFromStorage()); 
        window.addEventListener("storage", onStorage); 
        return () => window.removeEventListener("storage", onStorage); 
    }, []);

    const frontEndlogin = (token: string, user: AuthUser) => {
        localStorage.setItem(TOKEN_KEY, token);
        localStorage.setItem(USER_KEY, JSON.stringify(user));
        setState({ token, user, isAuthed: true });
    };

    const frontEndlogout = () => {
        localStorage.removeItem(TOKEN_KEY);
        localStorage.removeItem(USER_KEY);
        setState({ token: null, user: null, isAuthed: false });
    };

    const value = useMemo(() => ({ ...state, frontEndlogin, frontEndlogout }), [state]);

    return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

// ---- Hook ----
export function useAuth() {
    const ctx = useContext(AuthContext);
    if (!ctx) throw new Error("useAuth must be used inside <AuthProvider>");
    return ctx;
}