import { apiFetch } from "./api_client";

export interface ReceiptDashboard {
  id: number;
  receiptName: string | null;
  purchaseDate: string;         
  totalSaved: number;
  totalNumberOfItems: number;
  onSaleItems: number;
  // When the receipt was created in the system (for "Date added" column)
  createdAt?: string;
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

// ======== Create/Upload/Confirm flow ========

export interface ReceiptItemCreateDto {
  productSku: string | null;
  itemName: string;
  quantity: number;
  originalPrice: number;
  discountAmount: number;
  finalPrice: number;
  category: string | null;
}

export interface ReceiptCreateDto {
  storeName: string;
  receiptName?: string | null;
  purchaseDate: string; // ISO string
  totalAmount: number;
  totalNumberOfItems: number;
  imageUrl: string;
  notes?: string | null;
  items: ReceiptItemCreateDto[];
}

export interface UploadReceiptResponse {
  receiptId: number;
  imageUrl?: string;
}

export async function uploadReceipt(file: File): Promise<UploadReceiptResponse> {
  const base = process.env.NEXT_PUBLIC_API_URL || "";
  const token = typeof window !== "undefined" ? localStorage.getItem("token") : null;
  const formData = new FormData();
  formData.append("file", file);
  const res = await fetch(`${base}/api/receipt/upload`, {
    method: "POST",
    body: formData,
    headers: token ? { Authorization: `Bearer ${token}` } : undefined,
  });
  if (!res.ok) {
    const data = await res.json().catch(() => ({}));
    throw new Error(data.message || "Upload failed");
  }
  const data = await res.json().catch(() => ({}));
  const receiptId = data.receiptId ?? data.id ?? data.receiptID;
  if (typeof receiptId !== "number") throw new Error("Upload response missing receiptId");
  return { receiptId, imageUrl: data.imageUrl };
}

export async function processReceipt(receiptId: number): Promise<void> {
  await apiFetch<void>(`/api/receipt/${receiptId}/process`, { method: "POST" });
}

export async function getConfirmPreview(receiptId: number): Promise<ReceiptView> {
  return apiFetch<ReceiptView>(`/api/receipt/${receiptId}/confirm`);
}

export async function confirmReceipt(
  receiptId: number,
  payload: ReceiptCreateDto
): Promise<ReceiptView> {
  return apiFetch<ReceiptView>(`/api/receipt/${receiptId}/confirm`, {
    method: "POST",
    body: JSON.stringify(payload),
  });
}
