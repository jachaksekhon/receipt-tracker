"use client";

import { useEffect, useState } from "react";
import Modal from "@/components/ui/modal";
import { CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { getReceiptById, updateReceipt, ReceiptView } from "@/lib/services/receiptService";

interface Props {
  open: boolean;
  receiptId: number | null;
  onClose: () => void;
  onSaved?: (updated: ReceiptView) => void;
}

export default function ReceiptEditModal({ open, receiptId, onClose, onSaved }: Props) {
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [name, setName] = useState("");
  const [notes, setNotes] = useState("");

  useEffect(() => {
    let mounted = true;
    async function load() {
      if (!open || receiptId == null) return;
      setLoading(true);
      try {
        const res = await getReceiptById(receiptId);
        if (!mounted) return;
        setName(res.receiptName ?? "");
        setNotes(res.notes ?? "");
      } finally {
        if (mounted) setLoading(false);
      }
    }
    load();
    return () => {
      mounted = false;
    };
  }, [open, receiptId]);

  async function handleSave() {
    if (receiptId == null) return;
    setSaving(true);
    try {
      const updated = await updateReceipt(receiptId, {
        receiptName: name,
        notes,
      });
      onSaved?.(updated);
      onClose();
    } catch (e) {
      // surface via alert to keep simple
      alert((e as Error).message || "Failed to save changes.");
    } finally {
      setSaving(false);
    }
  }

  return (
    <Modal open={open} onClose={onClose} containerClassName="w-full max-w-xl mx-auto">
      <CardHeader className="flex-row items-center justify-between">
        <CardTitle>Edit receipt</CardTitle>
        <Button variant="outline" size="sm" onClick={onClose}>
          Close
        </Button>
      </CardHeader>
      <CardContent>
        {loading ? (
          <p className="text-sm text-muted-foreground">Loading…</p>
        ) : (
          <form
            className="space-y-4"
            onSubmit={(e) => {
              e.preventDefault();
              void handleSave();
            }}
          >
            <div className="grid gap-2">
              <label htmlFor="rname" className="text-sm">
                Receipt name
              </label>
              <input
                id="rname"
                className="h-9 rounded-md border bg-background px-3 py-1 text-sm outline-none focus:ring-2 focus:ring-primary/20"
                value={name}
                onChange={(e) => setName(e.target.value)}
                placeholder="e.g., Costco run"
              />
            </div>
            <div className="grid gap-2">
              <label htmlFor="rnotes" className="text-sm">
                Notes
              </label>
              <textarea
                id="rnotes"
                rows={4}
                className="rounded-md border bg-background px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-primary/20"
                value={notes}
                onChange={(e) => setNotes(e.target.value)}
                placeholder="Optional notes"
              />
            </div>
            <div className="flex justify-end gap-2">
              <Button type="button" variant="outline" onClick={onClose}>
                Cancel
              </Button>
              <Button type="submit" disabled={saving}>
                {saving ? "Saving…" : "Save changes"}
              </Button>
            </div>
          </form>
        )}
      </CardContent>
    </Modal>
  );
}

