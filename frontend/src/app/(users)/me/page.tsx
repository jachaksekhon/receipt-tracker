/* eslint-disable @typescript-eslint/no-explicit-any */
"use client";

import { useEffect, useMemo, useState } from "react";
import Link from "next/link";
import { getCurrentUser, UserProfile } from "@/lib/services/userService";
import {
  getUserReceipts,
  deleteReceipt,
  getReceiptById,
  updateReceipt,
  ReceiptDashboard,
  ReceiptView,
} from "@/lib/services/receiptService";
import {
  Card,
  CardHeader,
  CardTitle,
  CardContent,
} from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import ReceiptViewModal from "@/components/receipt/ReceiptViewModal";
import ReceiptEditModal from "@/components/receipt/ReceiptEditModal";
import {
  ArrowUpRight,
  DollarSign,
  Pencil,
  Trash2,
  User as UserIcon,
} from "lucide-react";

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
  const [user, setUser] = useState<UserProfile | null>(null);
  const [receipts, setReceipts] = useState<ReceiptDashboard[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [expanded, setExpanded] = useState(false);
  const [viewOpen, setViewOpen] = useState(false);
  const [editOpen, setEditOpen] = useState(false);
  const [activeId, setActiveId] = useState<number | null>(null);
  const [activeLoading, setActiveLoading] = useState(false);

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

  const totalSavedAllTime = useMemo(
    () => receipts.reduce((sum, r) => sum + (r.totalSaved || 0), 0),
    [receipts]
  );

  const totalReceipts = receipts.length;

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
      {/* Header: profile pic + welcome */}
      <Card className="overflow-hidden">
        <CardContent className="p-6">
          <div className="flex items-center gap-4">
            {/* Avatar */}
            <div className="size-16 rounded-full bg-muted/80 flex items-center justify-center text-lg font-semibold text-foreground/80">
              {user.firstName || user.lastName ? (
                <span>{initials(user.firstName, user.lastName)}</span>
              ) : (
                <UserIcon className="h-7 w-7" />
              )}
            </div>
            {/* Text block */}
            <div>
              <div className="text-xl font-semibold">Welcome, {user.firstName || name || "Friend"}</div>
              <div className="text-sm text-muted-foreground">{user.email}</div>
              <div className="text-xs text-muted-foreground">Joined {joined}</div>
            </div>
          </div>
        </CardContent>
      </Card>

      {/* Stat cards */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
        <Card>
          <CardContent className="p-6">
            <div className="text-sm text-muted-foreground mb-2">Total receipts</div>
            <div className="text-3xl font-semibold">{totalReceipts}</div>
            <div className="mt-3 text-xs text-muted-foreground">Another useful stat could go here.</div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-6">
            <div className="flex items-center justify-between">
              <div className="text-sm text-muted-foreground">Total saved since joining</div>
              <ArrowUpRight className="h-4 w-4 text-emerald-600" />
            </div>
            <div className="mt-1 flex items-center gap-2">
              <DollarSign className="h-7 w-7 text-emerald-600" />
              <div className="text-3xl font-semibold">{formatCurrency(totalSavedAllTime)}</div>
            </div>
            <div className="mt-3 text-xs text-muted-foreground">Great savings — keep it up!</div>
          </CardContent>
        </Card>
      </div>

      {/* Receipts table */}
      <Card>
        <CardHeader className="pb-3 flex items-center justify-between">
          <CardTitle>Your receipts</CardTitle>
          {receipts.length > 6 && (
            <Button
              size="sm"
              variant="outline"
              onClick={() => setExpanded((v) => !v)}
            >
              {expanded ? "Collapse" : "Expand"}
            </Button>
          )}
        </CardHeader>
        <CardContent>
          {receipts.length === 0 ? (
            <p className="text-sm text-muted-foreground">No receipts found.</p>
          ) : (
            <div className={expanded ? "overflow-x-auto" : "overflow-x-auto max-h-96 overflow-y-auto rounded-md"}>
              <table className="w-full border-separate border-spacing-0 text-sm">
                <thead className="sticky top-0 bg-background z-10">
                  <tr className="text-left text-muted-foreground">
                    <th className="sticky left-0 bg-background p-3 font-medium border-b z-20">Receipt</th>
                    <th className="p-3 font-medium border-b">Date of purchase</th>
                    <th className="p-3 font-medium border-b text-center">Items</th>
                    <th className="p-3 font-medium border-b text-center">On sale</th>
                    <th className="p-3 font-medium border-b">Saved</th>
                    <th className="p-3 font-medium border-b text-center">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {receipts.map((r, idx) => (
                    <tr
                      key={r.id}
                      className="group border-b hover:bg-muted/40 transition-colors"
                    >
                      <td className="p-3 font-medium">
                        <button
                          type="button"
                          onClick={() => openView(r.id)}
                          className="hover:underline text-left"
                        >
                          {r.receiptName ?? `Receipt ${r.id}`}
                        </button>
                      </td>
                      <td className="p-3 whitespace-nowrap">
                        {new Date(r.purchaseDate).toLocaleDateString()}
                      </td>
                      <td className="p-3 text-center">{r.totalNumberOfItems}</td>
                      <td className="p-3 text-center">{r.onSaleItems}</td>
                      <td className="p-3">{formatCurrency(r.totalSaved)}</td>
                      <td className="p-3 text-center">
                        <div className="flex justify-center items-center gap-2 opacity-80 group-hover:opacity-100">
                          <Button size="sm" variant="outline" className="h-8" onClick={() => openEdit(r.id)}>
                            <Pencil className="h-4 w-4" />
                          </Button>
                          <Button
                            size="sm"
                            variant="destructive"
                            className="h-8"
                            onClick={() => handleDelete(r.id)}
                          >
                            <Trash2 className="h-4 w-4" />
                          </Button>
                        </div>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          )}
        </CardContent>
      </Card>
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
