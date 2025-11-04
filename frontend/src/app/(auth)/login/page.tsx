"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { loginUser } from "@/lib/services/authService";
import LoginStrings from "@/constants/LoginStrings"

export default function LoginPage() {
  const router = useRouter();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);
    setLoading(true);

    try {
      const response = await loginUser(email.trim(), password);

      if (!response.success) throw new Error(response.message);
      router.push("/dashboard");
    } catch (err: any) {
      setError(err.message || "Login failed.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen w-full bg-gradient-to-b from-background to-muted/40 flex items-center justify-center px-4 py-12">
      <Card className="w-full max-w-md shadow-sm">
        <CardHeader className="space-y-2">
          <CardTitle className="text-2xl text-center">{LoginStrings.TITLE}</CardTitle>
          <p className="text-sm text-muted-foreground text-center">
            {LoginStrings.SUBTITLE}
          </p>
        </CardHeader>

        <CardContent>
          {error && (
            <div
              role="alert"
              className="mb-4 rounded-md border border-destructive/30 bg-destructive/10 px-3 py-2 text-sm text-destructive"
            >
              {error}
            </div>
          )}

          <form onSubmit={handleSubmit} className="space-y-4">
            <div className="grid gap-2">
              <Label htmlFor="email">{LoginStrings.EMAIL_LABEL}</Label>
              <Input
                id="email"
                name="email"
                type="email"
                placeholder={LoginStrings.EMAIL_PLACEHOLDER}
                value={email}
                onChange={(e) => setEmail(e.target.value.toLowerCase())}
                required
              />
            </div>

            <div className="grid gap-2">
              <div className="flex items-center justify-between">
                <Label htmlFor="password">{LoginStrings.PASSWORD_LABEL}</Label>
                <Link
                  href="#"
                  className="text-xs text-muted-foreground hover:text-foreground"
                >
                  {LoginStrings.FORGOT_PASSWORD}
                </Link>
              </div>
              <Input
                id="password"
                name="password"
                type="password"
                placeholder={LoginStrings.PASSWORD_PLACEHOLDER}
                value={password}
                onChange={(e) => setPassword(e.target.value)}
                required
              />
            </div>

            <Button type="submit" className="w-full" disabled={loading}>
              {loading ? LoginStrings.SUBMIT_LOADING : LoginStrings.SUBMIT_TEXT}
            </Button>
          </form>

          <div className="mt-6 text-center text-sm text-muted-foreground">
            {LoginStrings.FOOTER_TEXT}{" "}
            <Link href="/register" className="text-primary hover:underline">
              {LoginStrings.FOOTER_LINK}
            </Link>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
