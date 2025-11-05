"use client";

import { useEffect, useMemo, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { Button } from "@/components/ui/button";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";
import {
  getConfirmPreview,
  confirmReceipt,
  ReceiptItemCreateDto,
  ReceiptCreateDto,
  ReceiptView,
} from "@/lib/services/receiptService";
import { formatCurrency } from "@/lib/ui/format";
import { X } from "lucide-react";

function toDateInputValue(iso: string) {
  const d = new Date(iso);
  const year = d.getFullYear();
  const month = String(d.getMonth() + 1).padStart(2, "0");
  const day = String(d.getDate()).padStart(2, "0");
  return `${year}-${month}-${day}`;
}

export default function ConfirmReceiptPage() {
  const params = useParams<{ id: string }>();
  const router = useRouter();
  const receiptId = Number(params?.id);

  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const [storeName, setStoreName] = useState("");
  const [receiptName, setReceiptName] = useState<string>("");
  const [purchaseDate, setPurchaseDate] = useState<string>("");
  const [notes, setNotes] = useState<string>("");
  const [items, setItems] = useState<ReceiptItemCreateDto[]>([]);

  useEffect(() => {
    async function load() {
      try {
        const data: ReceiptView = await getConfirmPreview(receiptId);
        setStoreName(data.storeName);
        setReceiptName(data.receiptName ?? "");
        setPurchaseDate(toDateInputValue(data.purchaseDate));
        setNotes(data.notes ?? "");
        setItems(
          data.items.map((it) => ({
            productSku: it.productSku,
            itemName: it.itemName,
            quantity: it.quantity,
            originalPrice: it.originalPrice,
            discountAmount: it.discountAmount,
            finalPrice: it.finalPrice,
            category: it.category,
          }))
        );
      } catch (e: any) {
        setError(e.message || "Failed to load confirmation preview.");
      } finally {
        setLoading(false);
      }
    }
    if (!Number.isFinite(receiptId)) {
      setError("Invalid receipt id");
      setLoading(false);
      return;
    }
    load();
  }, [receiptId]);

  const totalAmount = useMemo(
    () => items.reduce((sum, it) => sum + Number(it.finalPrice || 0), 0),
    [items]
  );

  function updateItem(idx: number, key: keyof ReceiptItemCreateDto, value: string) {
    setItems((prev) => {
      const next = [...prev];
      const it = { ...next[idx] } as any;
      if (key === "quantity") {
        const n = value === "" ? 0 : parseInt(value, 10);
        it[key] = isNaN(n) ? 0 : Math.max(0, Math.min(999, n));
      } else if (key === "originalPrice" || key === "discountAmount" || key === "finalPrice") {
        const n = value === "" ? 0 : parseFloat(value);
        const rounded = Math.round((isNaN(n) ? 0 : n) * 100) / 100; // limit to 2 decimals
        it[key] = rounded;
      } else {
        it[key] = value;
      }
      next[idx] = it;
      return next;
    });
  }

  function addRow() {
    setItems((prev) => [
      ...prev,
      {
        productSku: "",
        itemName: "",
        quantity: 1,
        originalPrice: 0,
        discountAmount: 0,
        finalPrice: 0,
        category: null,
      },
    ]);
  }

  function removeRow(idx: number) {
    setItems((prev) => prev.filter((_, i) => i !== idx));
  }

  async function handleSubmit() {
    setSaving(true);
    try {
      const payload: ReceiptCreateDto = {
        storeName,
        receiptName: receiptName || null,
        purchaseDate: new Date(purchaseDate).toISOString(),
        totalAmount: Number(totalAmount),
        totalNumberOfItems: items.length,
        imageUrl: "",
        notes: notes || null,
        items,
      };
      await confirmReceipt(receiptId, payload);
      router.push("/me?refresh=1");
    } catch (e: any) {
      setError(e.message || "Failed to confirm receipt.");
    } finally {
      setSaving(false);
    }
  }

  if (loading) return <p className="text-center mt-8 text-sm text-muted-foreground">Loading preview...</p>;
  if (error) return <p className="text-center mt-8 text-sm text-destructive">{error}</p>;

  return (
    <div className="w-full flex justify-center py-10">
      <div className="w-full max-w-5xl px-4 md:px-6 space-y-6">
        <Card>
          <CardHeader>
            <CardTitle>Confirm receipt</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
              <div className="grid gap-2">
                <Label htmlFor="store">Store name</Label>
                <Input id="store" value={storeName} disabled readOnly />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="rname">Receipt name</Label>
                <Input id="rname" value={receiptName} onChange={(e) => setReceiptName(e.target.value)} placeholder="Optional" />
              </div>
              <div className="grid gap-2">
                <Label htmlFor="date">Date of purchase</Label>
                <Input id="date" type="date" value={purchaseDate} onChange={(e) => setPurchaseDate(e.target.value)} />
              </div>
              <div className="grid gap-2">
                <Label>Total amount</Label>
                <div className="h-9 rounded-md border bg-muted/30 px-3 flex items-center text-sm">{formatCurrency(totalAmount)}</div>
              </div>
            </div>

            <div className="grid gap-2">
              <Label htmlFor="notes">Notes</Label>
              <textarea id="notes" rows={3} className="rounded-md border bg-background px-3 py-2 text-sm outline-none focus:ring-2 focus:ring-primary/20" value={notes} onChange={(e) => setNotes(e.target.value)} />
            </div>

            <div className="flex items-center justify-between">
              <div className="text-sm font-medium">Items</div>
              <Button variant="outline" size="sm" onClick={addRow}>Add item</Button>
            </div>

            <div className="overflow-x-auto rounded-md max-h-[60vh] overflow-y-auto pb-24">
              <table className="w-full table-fixed border-separate border-spacing-0 text-sm">
                <thead className="sticky top-0 bg-background z-10">
                  <tr className="text-left text-muted-foreground">
                    <th className="p-3 font-medium border-b w-24">Product SKU</th>
                    <th className="p-3 font-medium border-b w-[45%]">Item</th>
                    <th className="p-3 font-medium border-b text-right w-12">Qty</th>
                    <th className="p-3 font-medium border-b text-right w-20">Original</th>
                    <th className="p-3 font-medium border-b text-right w-20">Discount</th>
                    <th className="p-3 font-medium border-b text-right w-20">Final</th>
                    <th className="p-3 font-medium border-b text-center w-10">Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {items.map((it, idx) => (
                    <tr key={idx} className="border-b">
                      <td className="p-2"><Input value={it.productSku ?? ""} onChange={(e) => updateItem(idx, "productSku", e.target.value)} className="w-24" /></td>
                      <td className="p-2"><Input value={it.itemName} onChange={(e) => updateItem(idx, "itemName", e.target.value)} className="w-full" /></td>
                      <td className="p-2"><Input type="number" min={0} max={999} value={it.quantity} onChange={(e) => updateItem(idx, "quantity", e.target.value)} className="text-right w-12" /></td>
                      <td className="p-2"><Input type="number" step="0.01" value={it.originalPrice} onChange={(e) => updateItem(idx, "originalPrice", e.target.value)} className="text-right w-20" /></td>
                      <td className="p-2"><Input type="number" step="0.01" value={it.discountAmount} onChange={(e) => updateItem(idx, "discountAmount", e.target.value)} className="text-right w-20" /></td>
                      <td className="p-2"><Input type="number" step="0.01" value={it.finalPrice} onChange={(e) => updateItem(idx, "finalPrice", e.target.value)} className="text-right w-20" /></td>
                      <td className="p-2 text-center">
                        <button
                          type="button"
                          onClick={() => removeRow(idx)}
                          className="text-destructive hover:underline"
                          aria-label="Remove row"
                          title="Remove"
                        >
                          ×
                        </button>
                      </td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>

            <div className="flex justify-end gap-2 sticky bottom-0 bg-background pt-2">
              <Button variant="outline" type="button" onClick={() => router.push("/me")}>Cancel</Button>
              <Button type="button" onClick={handleSubmit} disabled={saving}>{saving ? "Saving..." : "Done"}</Button>
            </div>
          </CardContent>
        </Card>
      </div>
    </div>
  );
}
