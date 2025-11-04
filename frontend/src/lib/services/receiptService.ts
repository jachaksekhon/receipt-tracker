import { apiFetch } from "./api_client";

export interface ReceiptDashboard {
  id: number;
  receiptName: string | null;
  purchaseDate: string;         
  totalSaved: number;
  totalNumberOfItems: number;
  onSaleItems: number;
}

// ========== Receipt details ==========
export interface ReceiptItemRead {
  id: number;
  productSku: string | null;
  itemName: string;
  quantity: number;
  originalPrice: number;
  discountAmount: number;
  finalPrice: number;
  category: string | null;
}

export interface ReceiptView {
  id: number;
  receiptName: string | null;
  storeName: string;
  purchaseDate: string;
  totalAmount: number;
  totalNumberOfItems: number;
  notes: string | null;
  items: ReceiptItemRead[];
}

export async function getUserReceipts(): Promise<ReceiptDashboard[]> {
  return apiFetch<ReceiptDashboard[]>("/api/receipt/all");
}

export async function getReceiptById(id: number): Promise<ReceiptView> {
  return apiFetch<ReceiptView>(`/api/receipt/${id}`);
}

export async function deleteReceipt(id: number): Promise<void> {
  await apiFetch<void>(`/api/receipt/${id}`, { method: "DELETE" });
}

// Lightweight update for editing a receipt (e.g., name/notes)
export interface ReceiptUpdate {
  receiptName?: string | null;
  notes?: string | null;
}

export async function updateReceipt(
  id: number,
  payload: ReceiptUpdate
): Promise<ReceiptView> {
  return apiFetch<ReceiptView>(`/api/receipt/${id}`, {
    method: "PUT",
    body: JSON.stringify(payload),
  });
}
