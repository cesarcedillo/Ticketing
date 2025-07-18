import { useState } from "react";
import { markTicketAsResolved } from "../api/ticketApi";

export function useMarkTicketAsResolved() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const resolve = async (ticketId: string) => {
    setLoading(true);
    setError(null);
    try {
      await markTicketAsResolved(ticketId);
    } catch (e: any) {
      setError(e.message || "Error resolviendo ticket");
      throw e;
    }
    setLoading(false);
  };

  return { loading, error, resolve };
}
