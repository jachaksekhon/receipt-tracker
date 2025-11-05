"use client";

import { Card, CardContent } from "@/components/ui/card";
import { ArrowUpRight, DollarSign } from "lucide-react";
import { formatCurrency } from "@/lib/ui/format";

interface Props {
  totalReceipts: number;
  totalSaved: number;
}

export default function StatsCards({ totalReceipts, totalSaved }: Props) {
  return (
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
            <div className="text-3xl font-semibold">{formatCurrency(totalSaved)}</div>
          </div>
          <div className="mt-3 text-xs text-muted-foreground">Great savings — keep it up!</div>
        </CardContent>
      </Card>
    </div>
  );
}

