"use client";

import { ReactNode, useEffect } from "react";
import { Card } from "@/components/ui/card";

interface ModalProps {
  open: boolean;
  onClose: () => void;
  children: ReactNode;
  containerClassName?: string;
  overlayClassName?: string;
  closeOnOverlay?: boolean;
}

export default function Modal({
  open,
  onClose,
  children,
  containerClassName = "w-full max-w-2xl mx-auto",
  overlayClassName = "bg-black/40",
  closeOnOverlay = true,
}: ModalProps) {
  useEffect(() => {
    if (!open) return;
    const onKey = (e: KeyboardEvent) => {
      if (e.key === "Escape") onClose();
    };
    document.addEventListener("keydown", onKey);
    document.body.style.overflow = "hidden";
    return () => {
      document.removeEventListener("keydown", onKey);
      // Always clear overflow on cleanup to avoid stuck scroll lock
      document.body.style.overflow = "";
    };
  }, [open, onClose]);

  if (!open) return null;

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center">
      <div
        className={`absolute inset-0 ${overlayClassName}`}
        onClick={closeOnOverlay ? onClose : undefined}
      />
      <div role="dialog" aria-modal="true" className={`relative ${containerClassName}`}>
        <Card className="shadow-lg">{children}</Card>
      </div>
    </div>
  );
}
