// Instead of having the header in each file, we can use this file to inject layouts and keep the header here

import { Outlet } from "react-router-dom";
import Header from "./Header";

export default function AppLayout() {
    return (
        <div className="min-h-screen flex flex-col">
            <Header />
            <main className="flex1">
                <Outlet />
            </main>
        </div>
    )
}