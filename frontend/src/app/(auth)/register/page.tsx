/* eslint-disable @typescript-eslint/no-explicit-any */
"use client";

import { useState } from "react";
import Link from "next/link";
import { useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import { registerUser } from "@/lib/services/authService"
import RegisterStrings from "@/constants/RegisterStrings";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";

export default function RegisterPage() {
  const router = useRouter();

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirm, setShowConfirm] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError(null);

    if (password !== confirmPassword) {
      setError(RegisterStrings.ERROR_PASSWORD_MISMATCH);
      return;
    }

    setLoading(true);
    try {
        const response = await registerUser({
        firstName: firstName.trim(),
        lastName: lastName.trim(),
        email: email.trim(),
        password,
      });
      
      if (!response.success) throw new Error(response.message);

      router.push("/login");
    } catch (err: any) {
      setError(err.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-h-screen w-full bg-gradient-to-b from-background to-muted/40 flex items-center justify-center px-4 py-12">
      <Card className="w-full max-w-md shadow-sm">
        <CardHeader className="space-y-2">
          <CardTitle className="text-2xl text-center">{RegisterStrings.TITLE}</CardTitle>
          <p className="text-sm text-muted-foreground text-center">
            {RegisterStrings.SUBTITLE}
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
            <div className="grid grid-cols-1 gap-4 sm:grid-cols-2">
              <div className="grid gap-2">
                <Label htmlFor="firstName">{RegisterStrings.FIRST_NAME_LABEL}</Label>
                <Input
                  id="firstName"
                  name="firstName"
                  type="text"
                  placeholder={RegisterStrings.FIRST_NAME_PLACEHOLDER}
                  value={firstName}
                  onChange={(e) => setFirstName(e.target.value)}
                  autoComplete="given-name"
                  autoCapitalize="words"
                  required
                />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="lastName">{RegisterStrings.LAST_NAME_LABEL}</Label>
                <Input
                  id="lastName"
                  name="lastName"
                  type="text"
                  placeholder={RegisterStrings.LAST_NAME_PLACEHOLDER}
                  value={lastName}
                  onChange={(e) => setLastName(e.target.value)}
                  autoComplete="family-name"
                  autoCapitalize="words"
                  required
                />
              </div>
            </div>
            <div className="grid gap-2">
              <Label htmlFor="email">{RegisterStrings.EMAIL_LABEL}</Label>
              <Input
                id="email"
                name="email"
                type="email"
                placeholder={RegisterStrings.EMAIL_PLACEHOLDER}
                value={email}
                onChange={(e) => setEmail(e.target.value.toLowerCase())}
                autoComplete="email"
                required
              />
            </div>

            <div className="grid gap-2">
              <div className="flex items-center justify-between">
                <Label htmlFor="password">{RegisterStrings.PASSWORD_LABEL}</Label>
                <span className="text-xs text-muted-foreground">{RegisterStrings.PASSWORD_HINT}</span>
              </div>
              <div className="relative">
                <Input
                  id="password"
                  name="password"
                  type={showPassword ? "text" : "password"}
                  placeholder={RegisterStrings.PASSWORD_PLACEHOLDER}
                  value={password}
                  onChange={(e) => setPassword(e.target.value)}
                  autoComplete="new-password"
                  minLength={8}
                  required
                />
                <button
                  type="button"
                  onClick={() => setShowPassword((s) => !s)}
                  className="absolute inset-y-0 right-2 my-auto inline-flex items-center rounded px-2 text-xs text-muted-foreground hover:text-foreground"
                  aria-label={showPassword ? "Hide password" : "Show password"}
                >
                  {showPassword ? RegisterStrings.HIDE_PASSWORD : RegisterStrings.SHOW_PASSWORD}
                </button>
              </div>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="confirmPassword">{RegisterStrings.CONFIRM_PASSWORD_LABEL}</Label>
              <div className="relative">
                <Input
                  id="confirmPassword"
                  name="confirmPassword"
                  type={showConfirm ? "text" : "password"}
                  placeholder={RegisterStrings.CONFIRM_PASSWORD_PLACEHOLDER}
                  value={confirmPassword}
                  onChange={(e) => setConfirmPassword(e.target.value)}
                  autoComplete="new-password"
                  minLength={8}
                  required
                />
                <button
                  type="button"
                  onClick={() => setShowConfirm((s) => !s)}
                  className="absolute inset-y-0 right-2 my-auto inline-flex items-center rounded px-2 text-xs text-muted-foreground hover:text-foreground"
                  aria-label={showConfirm ? "Hide password" : "Show password"}
                >
                  {showConfirm ? RegisterStrings.HIDE_CONFIRM : RegisterStrings.SHOW_CONFIRM}
                </button>
              </div>
            </div>

            <Button type="submit" className="w-full" disabled={loading}>
              {loading ? RegisterStrings.SUBMIT_LOADING : RegisterStrings.SUBMIT_TEXT}
            </Button>
          </form>

          <div className="mt-6 text-center text-sm text-muted-foreground">
            {RegisterStrings.FOOTER_TEXT}{" "}
            <Link href="/login" className="text-primary hover:underline">
              {RegisterStrings.FOOTER_LINK}
            </Link>
          </div>
        </CardContent>
      </Card>
    </div>
  );
}
