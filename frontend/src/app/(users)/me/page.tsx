"use client";

import { useEffect, useMemo, useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import ProfileHeaderCard from "@/components/dashboard/ProfileHeaderCard";
import StatsCards from "@/components/dashboard/StatsCards";
import UploadReceiptCard from "@/components/dashboard/UploadReceiptCard";
import ReceiptsTableCard from "@/components/dashboard/ReceiptsTableCard";
import { getCurrentUser, UserProfile } from "@/lib/services/userService";
import { getUserReceipts, deleteReceipt, ReceiptDashboard } from "@/lib/services/receiptService";
import ReceiptViewModal from "@/components/receipt/ReceiptViewModal";
// Edit now routes to the confirm page; no separate edit modal

export default function UserDashboardPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const [user, setUser] = useState<UserProfile | null>(null);
  const [receipts, setReceipts] = useState<ReceiptDashboard[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [viewOpen, setViewOpen] = useState(false);
  // No local edit modal state; editing routes to confirm page
  const [activeId, setActiveId] = useState<number | null>(null);

  useEffect(() => {
    async function fetchData() {
      try {
        const [userData, receiptsData] = await Promise.all([
          getCurrentUser(),
          getUserReceipts(),
        ]);
        setUser(userData);
        setReceipts(receiptsData);
      } catch (err: any) {
        setError(err.message || "Failed to load dashboard.");
      } finally {
        setLoading(false);
      }
    }
    fetchData();
  }, []);

  // Helper to refresh receipts on demand
  async function refreshReceipts() {
    try {
      const data = await getUserReceipts();
      setReceipts(data);
    } catch (e) {
      // ignore for passive refresh
    }
  }

  // When returning with ?refresh=1, re-fetch receipts
  useEffect(() => {
    if (searchParams?.get("refresh")) {
      void refreshReceipts();
    }
  }, [searchParams]);

  // Also refresh when tab regains focus
  useEffect(() => {
    const onFocus = () => void refreshReceipts();
    const onVisibility = () => {
      if (document.visibilityState === "visible") void refreshReceipts();
    };
    window.addEventListener("focus", onFocus);
    document.addEventListener("visibilitychange", onVisibility);
    return () => {
      window.removeEventListener("focus", onFocus);
      document.removeEventListener("visibilitychange", onVisibility);
    };
  }, []);

  const totalSavedAllTime = useMemo(
    () => receipts.reduce((sum, r) => sum + (r.totalSaved || 0), 0),
    [receipts]
  );

  const totalReceipts = receipts.length;

  // Sorting/upload helpers are encapsulated in child components

  async function handleDelete(id: number) {
    const row = receipts.find((r) => r.id === id);
    const name = row?.receiptName || `#${id}`;
    const ok = confirm(`Delete receipt ${name}? This cannot be undone.`);
    if (!ok) return;
    try {
      await deleteReceipt(id);
      setReceipts((prev) => prev.filter((r) => r.id !== id));
    } catch (err: any) {
      alert(err.message || "Failed to delete receipt.");
    }
  }

  function openView(id: number) {
    setActiveId(id);
    setViewOpen(true);
  }

  function openEdit(id: number) {
    router.push(`/receipts/${id}/confirm`);
  }

  // Edits happen on confirm page; no local patching needed

  if (loading)
    return <p className="text-center mt-8 text-sm text-muted-foreground">Loading dashboard...</p>;
  if (error)
    return <p className="text-center mt-8 text-sm text-destructive">{error}</p>;
  if (!user) return null;

  return (
    <div className="w-full flex justify-center">
      <div className="w-full max-w-6xl px-4 md:px-6 space-y-6">
      <ProfileHeaderCard user={user} />

      <StatsCards totalReceipts={totalReceipts} totalSaved={totalSavedAllTime} />

        <UploadReceiptCard onProcessed={(id) => router.push(`/receipts/${id}/confirm`)} />

      <ReceiptsTableCard
        receipts={receipts}
        onView={openView}
        onEdit={openEdit}
        onDelete={handleDelete}
      />
      </div>

      <ReceiptViewModal open={viewOpen} receiptId={activeId} onClose={() => setViewOpen(false)} />
      {/* Editing is handled on /receipts/[id]/confirm */}

      
    </div>
  );
}


