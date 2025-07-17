import { useState } from "react";
import { postTicketReply } from "../api/ticketApi";

export function usePostTicketReply() {
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const sendReply = async (ticketId: string, text: string, userId: string) => {
    setLoading(true);
    setError(null);
    try {
      await postTicketReply(ticketId, text, userId);
    } catch (e: any) {
      setError(e.message || "Error sending reply");
    }
    setLoading(false);
  };

  return { loading, error, sendReply };
}