"use client";

import { useEffect, useState } from "react";
import Modal from "@/components/ui/modal";
import { CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { getReceiptById, ReceiptView } from "@/lib/services/receiptService";

function formatCurrency(n: number) {
  return new Intl.NumberFormat(undefined, {
    style: "currency",
    currency: "USD",
    maximumFractionDigits: 2,
  }).format(n ?? 0);
}

interface Props {
  open: boolean;
  receiptId: number | null;
  onClose: () => void;
}

export default function ReceiptViewModal({ open, receiptId, onClose }: Props) {
  const [loading, setLoading] = useState(false);
  const [data, setData] = useState<ReceiptView | null>(null);

  useEffect(() => {
    let mounted = true;
    async function load() {
      if (!open || receiptId == null) return;
      setLoading(true);
      try {
        const res = await getReceiptById(receiptId);
        if (mounted) setData(res);
      } catch (e) {
        if (mounted) setData(null);
      } finally {
        if (mounted) setLoading(false);
      }
    }
    load();
    return () => {
      mounted = false;
    };
  }, [open, receiptId]);

  return (
    <Modal open={open} onClose={onClose} containerClassName="w-full max-w-4xl mx-auto">
      <CardHeader className="flex-row items-center justify-between">
        <CardTitle>Receipt details</CardTitle>
        <Button variant="outline" size="sm" onClick={onClose}>
          Close
        </Button>
      </CardHeader>
      <CardContent>
        {loading || !data ? (
          <p className="text-sm text-muted-foreground">Loading receipt…</p>
        ) : (
          <div className="space-y-4">
            <div className="grid grid-cols-1 sm:grid-cols-2 gap-3 text-sm">
              <div>
                <span className="text-muted-foreground">Name:</span> {data.receiptName ?? `Receipt ${data.id}`}
              </div>
              <div>
                <span className="text-muted-foreground">Store:</span> {data.storeName}
              </div>
              <div>
                <span className="text-muted-foreground">Purchased:</span> {new Date(data.purchaseDate).toLocaleString()}
              </div>
              <div>
                <span className="text-muted-foreground">Total:</span> {formatCurrency(data.totalAmount)}
              </div>
              {data.notes && (
                <div className="sm:col-span-2">
                  <span className="text-muted-foreground">Notes:</span> {data.notes}
                </div>
              )}
            </div>

            <div className="overflow-x-auto max-h-[60vh] overflow-y-auto rounded-md">
              <table className="w-full border-separate border-spacing-0 text-sm">
                <thead className="sticky top-0 bg-background z-10">
                  <tr className="text-left text-muted-foreground">
                    <th className="p-3 font-medium border-b">Item</th>
                    <th className="p-3 font-medium border-b">Qty</th>
                    <th className="p-3 font-medium border-b">Original</th>
                    <th className="p-3 font-medium border-b">Discount</th>
                    <th className="p-3 font-medium border-b">Final</th>
                    <th className="p-3 font-medium border-b">Category</th>
                  </tr>
                </thead>
                <tbody>
                  {data.items.map((it) => (
                    <tr key={it.id} className="border-b hover:bg-muted/40 transition-colors">
                      <td className="p-3 font-medium">{it.itemName}</td>
                      <td className="p-3">{it.quantity}</td>
                      <td className="p-3">{formatCurrency(it.originalPrice)}</td>
                      <td className="p-3">{formatCurrency(it.discountAmount)}</td>
                      <td className="p-3">{formatCurrency(it.finalPrice)}</td>
                      <td className="p-3">{it.category ?? "—"}</td>
                    </tr>
                  ))}
                </tbody>
              </table>
            </div>
          </div>
        )}
      </CardContent>
    </Modal>
  );
}

