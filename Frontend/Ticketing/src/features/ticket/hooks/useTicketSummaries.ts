import { useCallback, useEffect, useState } from "react";
import { fetchTicketSummaries } from "../api/ticketApi";
import type { TicketSummary } from "../types/Ticket";

export function useTicketSummaries() {
  const [tickets, setTickets] = useState<TicketSummary[]>([]);
  const [loading, setLoading] = useState<boolean>(false);
  const [error, setError] = useState<string>("");
  
  const loadTickets = useCallback(async () => {
    setLoading(true);
    setError("");
    try {
      const result = await fetchTicketSummaries();
      setTickets(result);
    } catch {
      setError("Error loading tickets");
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    loadTickets();
  }, [loadTickets]);

  return {
    tickets,
    loading,
    error,
    refetch: loadTickets,
  };
}
