import { useCallback, useState } from "react";
import { fetchTicketDetail } from "../api/ticketApi";
import type { Ticket } from "../types/Ticket";

export function useTicketDetail() {
  const [ticket, setTicket] = useState<Ticket | undefined>();
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>("");

  const loadTicket = useCallback(async (id: string) => {
    setLoading(true);
    setError("");
    try {
      const t = await fetchTicketDetail(id);
      setTicket(t);
    } catch {
      setError("Error loading ticket details");
      setTicket(undefined);
    } finally {
      setLoading(false);
    }
  }, []);

  return { ticket, loading, error, loadTicket };
}
