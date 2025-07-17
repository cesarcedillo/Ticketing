import type { TicketSummary } from "../types/Ticket";
import { bytesToImageUrl } from "../../../shared/utils/bytesToImageUrl";
import { useEffect, useState } from "react";

type Props = {
  tickets: TicketSummary[];
  selectedId: string | null;
  onSelect: (id: string) => void;
};

export default function TicketList({ tickets, selectedId, onSelect }: Props) {
  const [avatars, setAvatars] = useState<Record<string, string>>({});

  useEffect(() => {
    const urls: Record<string, string> = {};
    tickets.forEach(ticket => {
      if (ticket.avatar?.length) {
        const url = bytesToImageUrl(ticket.avatar);
        if (url) urls[ticket.id] = url;
      }
    });
    setAvatars(urls);
    return () => {
      Object.values(urls).forEach(url => URL.revokeObjectURL(url));
    };
  }, [tickets]);

  return (
    <div>
      <h3>Tickets</h3>
      <ul style={{ listStyle: "none", padding: 0 }}>
        {tickets.map(ticket => (
          <li
            key={ticket.id}
            onClick={() => onSelect(ticket.id)}
            style={{
              display: "flex", alignItems: "center", gap: 10,
              padding: "0.7em 1em", marginBottom: 4, cursor: "pointer",
              background: ticket.id === selectedId ? "#e3e8f0" : "white",
              borderRadius: 6,
              border: ticket.id === selectedId ? "2px solid #3182ce" : "1px solid #e2e8f0"
            }}
          >
            {avatars[ticket.id] && (
              <img src={avatars[ticket.id]} alt="avatar"
                style={{ width: 30, height: 30, borderRadius: "50%" }} />
            )}
            <div style={{ flex: 1 }}>
              <b>{ticket.subject}</b><br />
              <small>{ticket.userName} - {ticket.userId}</small>
            </div>
          </li>
        ))}
      </ul>
    </div>
  );
}
