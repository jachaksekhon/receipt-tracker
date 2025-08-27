import { Link, NavLink, useNavigate } from "react-router-dom";
import { useAuth } from "@/auth/useAuth";
import { Button } from "@/components/ui/button"; // if you’re using shadcn buttons; else use <button>
import LoginStrings from "@/constants/strings/LoginStrings";

// Example nav items
const nav = [
  { to: "/", label: "Home" },
  { to: "/dashboard", label: "Dashboard" },
];

export default function Header() {
    const { isAuthed, user, frontEndlogout } = useAuth();
    const navigate = useNavigate();

    const linkClass = ({ isActive }: { isActive: boolean }) =>
        `px-2 py-1 rounded-md transition-colors ${
        isActive ? "text-blue-600 font-semibold" : "text-slate-700 hover:text-blue-600"
        }`;

    const handleLogout = () => {
        frontEndlogout();
        navigate('/');
    }

    return (
        <header className="sticky top-0 z-40 w-full bg-white border-b">
            <div className="mx-auto max-w-6xl px-4 py-3 flex items-center justify-between">
                <Link to="/" className="text-xl font-bold">
                Receipt Tracker
                </Link>

                <nav className="flex items-center gap-6">
                {nav.map((item) => (
                    <NavLink key={item.to} to={item.to} className={ linkClass }>
                    {item.label}
                    </NavLink>
                ))}
                </nav>

                <div className="flex items-center gap-3">
                {isAuthed ? (
                    <>
                    <span className="text-sm text-slate-600">
                        Hi, {user?.name || user?.email}
                    </span>
                    <Button variant="outline" onClick={ handleLogout }>
                        {LoginStrings.Logout}
                    </Button>
                    </>
                ) : (
                    <>
                    <Button variant="outline" asChild>
                        <Link to="/login">{LoginStrings.LoginButton}</Link>
                    </Button>
                    <Button asChild>
                        <Link to="/signup">{LoginStrings.SignUp}</Link>
                    </Button>
                    </>
                )}
                </div>
            </div>
        </header>
    );
}
