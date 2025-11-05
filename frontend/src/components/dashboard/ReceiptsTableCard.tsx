"use client";

import Link from "next/link";
import { useMemo, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import { ArrowUpDown, ChevronDown, ChevronUp, Pencil, Trash2 } from "lucide-react";
import type { ReceiptDashboard } from "@/lib/services/receiptService";
import { formatCurrency } from "@/lib/ui/format";

interface Props {
  receipts: ReceiptDashboard[];
  onView: (id: number) => void;
  onEdit: (id: number) => void;
  onDelete: (id: number) => void;
}

export default function ReceiptsTableCard({ receipts, onView, onEdit, onDelete }: Props) {
  const [expanded, setExpanded] = useState(false);
  const [sortKey, setSortKey] = useState<"date" | "saved" | null>(null);
  const [sortDir, setSortDir] = useState<"asc" | "desc">("desc");

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

  return (
    <Card>
      <CardHeader className="pb-3 flex items-center justify-between">
        <CardTitle>Your receipts</CardTitle>
        {receipts.length > 6 && (
          <Button size="sm" variant="outline" onClick={() => setExpanded((v) => !v)}>
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
                  <th className="p-3 font-medium border-b">
                    <Button
                      type="button"
                      variant="ghost"
                      size="sm"
                      className="inline-flex items-center gap-1 px-0 h-auto hover:text-foreground"
                      onClick={() => toggleSort("date")}
                      aria-sort={sortKey === "date" ? (sortDir === "asc" ? "ascending" : "descending") : "none"}
                    >
                      Date of purchase
                      {sortKey === "date" ? (sortDir === "asc" ? <ChevronUp className="h-4 w-4" /> : <ChevronDown className="h-4 w-4" />) : <ArrowUpDown className="h-4 w-4" />}
                    </Button>
                  </th>
                  <th className="p-3 font-medium border-b text-center">Items</th>
                  <th className="p-3 font-medium border-b text-center">On sale</th>
                  <th className="p-3 font-medium border-b">
                    <Button
                      type="button"
                      variant="ghost"
                      size="sm"
                      className="inline-flex items-center gap-1 px-0 h-auto hover:text-foreground"
                      onClick={() => toggleSort("saved")}
                      aria-sort={sortKey === "saved" ? (sortDir === "asc" ? "ascending" : "descending") : "none"}
                    >
                      Saved
                      {sortKey === "saved" ? (sortDir === "asc" ? <ChevronUp className="h-4 w-4" /> : <ChevronDown className="h-4 w-4" />) : <ArrowUpDown className="h-4 w-4" />}
                    </Button>
                  </th>
                  <th className="p-3 font-medium border-b text-center">Actions</th>
                </tr>
              </thead>
              <tbody>
                {sortedReceipts.map((r) => (
                  <tr key={r.id} className="group border-b hover:bg-muted/40 transition-colors">
                    <td className="p-3 font-medium">
                      <Button
                        type="button"
                        variant="link"
                        size="sm"
                        className="p-0 h-auto text-left font-medium"
                        onClick={() => onView(r.id)}
                      >
                        {r.receiptName ?? `Receipt ${r.id}`}
                      </Button>
                    </td>
                    <td className="p-3 whitespace-nowrap">{new Date(r.purchaseDate).toLocaleDateString()}</td>
                    <td className="p-3 text-center">{r.totalNumberOfItems}</td>
                    <td className="p-3 text-center">{r.onSaleItems}</td>
                    <td className="p-3">{formatCurrency(r.totalSaved)}</td>
                    <td className="p-3 text-center">
                      <div className="flex justify-center items-center gap-2 opacity-80 group-hover:opacity-100">
                        <Button size="sm" variant="outline" className="h-8" onClick={() => onEdit(r.id)}>
                          <Pencil className="h-4 w-4" />
                        </Button>
                        <Button size="sm" variant="destructive" className="h-8" onClick={() => onDelete(r.id)}>
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
  );
}

