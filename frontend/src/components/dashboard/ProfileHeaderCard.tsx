"use client";

import { Card, CardContent } from "@/components/ui/card";
import { User as UserIcon } from "lucide-react";
import type { UserProfile } from "@/lib/services/userService";
import { initials } from "@/lib/ui/format";

interface Props {
  user: UserProfile;
}

export default function ProfileHeaderCard({ user }: Props) {
  const joined = new Date(user.createdAt).toLocaleDateString();
  const name = `${user.firstName} ${user.lastName}`.trim();

  return (
    <Card className="overflow-hidden">
      <CardContent className="p-6">
        <div className="flex items-center gap-4">
          <div className="size-16 rounded-full bg-muted/80 flex items-center justify-center text-lg font-semibold text-foreground/80">
            {user.firstName || user.lastName ? (
              <span>{initials(user.firstName, user.lastName)}</span>
            ) : (
              <UserIcon className="h-7 w-7" />
            )}
          </div>
          <div>
            <div className="text-xl font-semibold">Welcome, {user.firstName || name || "Friend"}</div>
            <div className="text-sm text-muted-foreground">{user.email}</div>
            <div className="text-xs text-muted-foreground">Joined {joined}</div>
          </div>
        </div>
      </CardContent>
    </Card>
  );
}

