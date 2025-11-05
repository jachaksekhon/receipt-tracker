"use client";

import { useEffect, useState } from "react";
import { Card, CardContent, CardHeader, CardTitle } from "@/components/ui/card";
import { Button } from "@/components/ui/button";
import Modal from "@/components/ui/modal";
import { UploadCloud } from "lucide-react";

interface Props {
  onUploaded?: (file: File) => Promise<void> | void;
  onProcessed?: (receiptId: number) => void;
}

import { uploadReceipt, processReceipt } from "@/lib/services/receiptService";

export default function UploadReceiptCard({ onUploaded, onProcessed }: Props) {
  const [uploadOpen, setUploadOpen] = useState(false);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [file, setFile] = useState<File | null>(null);
  const [previewUrl, setPreviewUrl] = useState<string | null>(null);
  const [uploading, setUploading] = useState(false);
  const [processing, setProcessing] = useState(false);
  const [uploadedReceiptId, setUploadedReceiptId] = useState<number | null>(null);

  function onPickFile(e: React.ChangeEvent<HTMLInputElement>) {
    const f = e.target.files?.[0] || null;
    if (previewUrl) URL.revokeObjectURL(previewUrl);
    setFile(f);
    setPreviewUrl(f ? URL.createObjectURL(f) : null);
  }

  useEffect(() => {
    return () => {
      if (previewUrl) URL.revokeObjectURL(previewUrl);
    };
  }, [previewUrl]);

  async function handleUpload() {
    if (!file) return;
    setUploading(true);
    try {
      await onUploaded?.(file);
      const resp = await uploadReceipt(file);
      setUploadedReceiptId(resp.receiptId);
      setConfirmOpen(true);
    } catch (e: any) {
      alert(e.message || "Upload failed");
    } finally {
      setUploading(false);
    }
  }

  async function handleProcess() {
    if (uploadedReceiptId == null) return;
    setProcessing(true);
    try {
      await processReceipt(uploadedReceiptId);
      onProcessed?.(uploadedReceiptId);
      setConfirmOpen(false);
      setUploadOpen(false);
      setFile(null);
      if (previewUrl) URL.revokeObjectURL(previewUrl);
      setPreviewUrl(null);
    } catch (e: any) {
      alert(e.message || "Failed to start processing");
    } finally {
      setProcessing(false);
    }
  }

  return (
    <>
      <Card role="button" onClick={() => setUploadOpen(true)} className="border-dashed hover:bg-muted/40 transition-colors">
        <CardContent className="p-6 flex items-center justify-center">
          <div className="inline-flex items-center gap-2 text-sm font-medium text-muted-foreground">
            <UploadCloud className="h-4 w-4" />
            <span>+ Upload Receipt</span>
          </div>
        </CardContent>
      </Card>

      <Modal open={uploadOpen} onClose={() => setUploadOpen(false)} containerClassName="w-full max-w-xl mx-auto">
        <CardHeader className="pb-2">
          <CardTitle>Upload receipt</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <input
              type="file"
              accept="image/png,image/jpeg,application/pdf,.png,.jpg,.jpeg,.pdf"
              onChange={onPickFile}
              className="block w-full text-sm file:mr-3 file:rounded-md file:border file:bg-background file:px-3 file:py-1.5 file:text-sm file:font-medium file:hover:bg-accent file:hover:text-accent-foreground"
            />
            {file && (
              <div className="space-y-2">
                <div className="text-sm text-muted-foreground">
                  Selected: {file.name} ({Math.round(file.size / 1024)} KB)
                </div>
                {previewUrl && file.type.startsWith("image/") && (
                  <img src={previewUrl} alt="Preview" className="max-h-80 w-auto rounded-md border" />
                )}
                {previewUrl && file.type === "application/pdf" && (
                  <iframe src={previewUrl} className="w-full h-80 rounded-md border" />
                )}
              </div>
            )}
            <div className="flex justify-end gap-2">
              <Button
                variant="outline"
                type="button"
                onClick={() => {
                  setFile(null);
                  if (previewUrl) URL.revokeObjectURL(previewUrl);
                  setPreviewUrl(null);
                }}
              >
                Clear
              </Button>
              <Button variant="outline" type="button" onClick={() => setUploadOpen(false)}>
                Cancel
              </Button>
              <Button type="button" disabled={!file || uploading} onClick={handleUpload}>
                {uploading ? "Uploading..." : "Upload"}
              </Button>
            </div>
          </div>
        </CardContent>
      </Modal>

      <Modal open={confirmOpen} onClose={() => setConfirmOpen(false)} containerClassName="w-full max-w-lg mx-auto">
        <CardHeader className="pb-2">
          <CardTitle>Confirm upload</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            <p className="text-sm text-muted-foreground">
              Please make sure your image is clear, contains the Costco title, clearly shows all products, and shows the purchase date.
            </p>
            <div className="flex justify-end gap-2">
              <Button variant="outline" type="button" onClick={() => setConfirmOpen(false)}>
                Back
              </Button>
              <Button type="button" onClick={() => void handleProcess()} disabled={processing}>
                {processing ? "Processing..." : "Continue"}
              </Button>
            </div>
          </div>
        </CardContent>
      </Modal>
    </>
  );
}
