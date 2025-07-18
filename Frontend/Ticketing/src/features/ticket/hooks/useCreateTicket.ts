import { useState } from "react";
import { createTicket } from "../api/ticketApi";

export function useCreateTicket() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const create = async (subject: string, description: string, userId: string) => {
    setLoading(true);
    setError(null);
    try {
      await createTicket(subject, description, userId);
    } catch (e: any) {
      setError(e.message || "Error creando ticket");
      throw e;
    }
    setLoading(false);
  };

  return { loading, error, create };
}
