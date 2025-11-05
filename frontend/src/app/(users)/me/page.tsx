"use client";

import { useEffect, useMemo, useState } from "react";
import { useRouter, useSearchParams } from "next/navigation";
import ProfileHeaderCard from "@/components/dashboard/ProfileHeaderCard";
import StatsCards from "@/components/dashboard/StatsCards";
import UploadReceiptCard from "@/components/dashboard/UploadReceiptCard";
import ReceiptsTableCard from "@/components/dashboard/ReceiptsTableCard";
import { getCurrentUser, UserProfile } from "@/lib/services/userService";
import {
  getUserReceipts,
  deleteReceipt,
  ReceiptDashboard,
  ReceiptView,
} from "@/lib/services/receiptService";
import ReceiptViewModal from "@/components/receipt/ReceiptViewModal";
import ReceiptEditModal from "@/components/receipt/ReceiptEditModal";


function initials(first?: string, last?: string) {
  const a = (first || "").trim()[0] || "";
  const b = (last || "").trim()[0] || "";
  return (a + b).toUpperCase() || "?";
}

function formatCurrency(n: number) {
  return new Intl.NumberFormat(undefined, {
    style: "currency",
    currency: "USD",
    maximumFractionDigits: 2,
  }).format(n ?? 0);
}

export default function UserDashboardPage() {
  const router = useRouter();
  const searchParams = useSearchParams();
  const [user, setUser] = useState<UserProfile | null>(null);
  const [receipts, setReceipts] = useState<ReceiptDashboard[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [expanded, setExpanded] = useState(false);
  const [viewOpen, setViewOpen] = useState(false);
  const [editOpen, setEditOpen] = useState(false);
  const [activeId, setActiveId] = useState<number | null>(null);
  const [activeLoading, setActiveLoading] = useState(false);
  const [sortKey, setSortKey] = useState<"date" | "saved" | null>(null);
  const [sortDir, setSortDir] = useState<"asc" | "desc">("desc");
  const [uploadOpen, setUploadOpen] = useState(false);
  const [uploadFile, setUploadFile] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);
  const [confirmUploadOpen, setConfirmUploadOpen] = useState(false);

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

  const sortedReceipts = useMemo(() => {
    if (!sortKey) return receipts;
    const arr = receipts.slice();
    arr.sort((a, b) => {
      let av = 0;
      let bv = 0;
      if (sortKey === "date") {
        av = new Date(a.purchaseDate).getTime();
        bv = new Date(b.purchaseDate).getTime();
      } else if (sortKey === "saved") {
        av = a.totalSaved ?? 0;
        bv = b.totalSaved ?? 0;
      }
      const cmp = av === bv ? 0 : av < bv ? -1 : 1;
      return sortDir === "asc" ? cmp : -cmp;
    });
    return arr;
  }, [receipts, sortKey, sortDir]);

  function toggleSort(key: "date" | "saved") {
    if (sortKey !== key) {
      setSortKey(key);
      setSortDir("desc");
    } else {
      setSortDir((d) => (d === "asc" ? "desc" : "asc"));
    }
  }

  function onPickFile(e: React.ChangeEvent<HTMLInputElement>) {
    const f = e.target.files?.[0] || null;
    if (previewUrl) URL.revokeObjectURL(previewUrl);
    setUploadFile(f);
    setPreviewUrl(f ? URL.createObjectURL(f) : null);
  }

  useEffect(() => {
    return () => {
      if (previewUrl) URL.revokeObjectURL(previewUrl);
    };
  }, [previewUrl]);

  async function handleUpload() {
    if (!uploadFile) return;
    // TODO: Wire to your backend endpoint.
    console.log("Uploading", uploadFile.name);
    setUploadOpen(false);
    setUploadFile(null);
    if (previewUrl) URL.revokeObjectURL(previewUrl);
    setPreviewUrl(null);
  }

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
    setActiveId(id);
    setEditOpen(true);
  }

  function handleSaved(updated: ReceiptView) {
    setReceipts((prev) =>
      prev.map((r) => (r.id === updated.id ? { ...r, receiptName: updated.receiptName } : r))
    );
  }

  if (loading)
    return <p className="text-center mt-8 text-sm text-muted-foreground">Loading dashboard...</p>;
  if (error)
    return <p className="text-center mt-8 text-sm text-destructive">{error}</p>;
  if (!user) return null;

  const joined = new Date(user.createdAt).toLocaleDateString();
  const name = `${user.firstName} ${user.lastName}`.trim();

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
      <ReceiptEditModal
        open={editOpen}
        receiptId={activeId}
        onClose={() => setEditOpen(false)}
        onSaved={handleSaved}
      />

      
    </div>
  );
}


