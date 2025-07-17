import { useEffect, useState } from "react";
import { fetchTicketSummaries } from "../api/ticketApi";
import type { TicketSummary } from "../types/Ticket";

export function useTicketSummaries() {
  const [tickets, setTickets] = useState<TicketSummary[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>("");

  useEffect(() => {
    setLoading(true);
    fetchTicketSummaries()
      .then(setTickets)
      .catch(() => setError("Error loading tickets"))
      .finally(() => setLoading(false));
  }, []);

  return { tickets, loading, error };
}
