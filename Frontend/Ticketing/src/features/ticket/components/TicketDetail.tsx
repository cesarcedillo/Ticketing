import { useEffect, useState } from "react";
import type { Ticket } from "../types/Ticket";
import ReplyForm from "./ReplyForm";
import { useMarkTicketAsResolved } from "../hooks/useMarkTicketAsResolved";

type Props = {
  ticket: Ticket | undefined;
  userId: string;
  onReplyAdded: () => void;
  onResolved: () => void;
};

export default function TicketDetail({ ticket, userId, onReplyAdded, onResolved }: Props) {
  const [avatarUrl, setAvatarUrl] = useState<string | undefined>();
  const { loading, error, resolve } = useMarkTicketAsResolved();

  useEffect(() => {
    if (ticket?.avatar) {
      setAvatarUrl(`data:image/png;base64,${ticket.avatar}`);
    } else {
      setAvatarUrl(undefined);
    }
  }, [ticket]);

  if (!ticket) {
    return <div>Selecciona un ticket de la lista</div>;
  }

  const handleResolve = async () => {
    try {
      await resolve(ticket.id);
      onResolved();
    } catch {
      // error handled by the hook
    }
  };

  return (
    <div>
      <h2>{ticket.subject}</h2>
      <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
        {avatarUrl && (
          <img
            src={avatarUrl}
            alt="avatar"
            style={{ width: 38, height: 38, borderRadius: "50%", border: "1px solid #ccc" }}
          />
        )}
        <span style={{ fontWeight: "bold" }}>{ticket.userName}</span>
        <span style={{ color: "#666", fontSize: 13, marginLeft: 8 }}>
          ({ticket.status})
        </span>
        {ticket.status !== "Resolved" && (
          <button
            onClick={handleResolve}
            style={{ marginLeft: "auto" }}
            disabled={loading}
          >
            {loading ? "Resolving..." : "Resolve"}
          </button>
        )}
      </div>
      {error && <div style={{ color: "red", margin: "8px 0" }}>{error}</div>}
      <p style={{ marginTop: 16 }}>{ticket.description}</p>
      <hr />
      <h4>Respuestas</h4>
      <ul style={{ listStyle: "none", padding: 0 }}>
        {ticket.replies.map(reply => (
          <li key={reply.id} style={{ marginBottom: 18 }}>
            <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
              {reply.avatar && (
                <img
                  src={`data:image/png;base64,${reply.avatar}`}
                  alt="avatar"
                  style={{ width: 28, height: 28, borderRadius: "50%" }}
                />
              )}
              <b>{reply.userName}</b>
              <span style={{ color: "#888", fontSize: 12 }}>
                {new Date(reply.createdAt).toLocaleString()}
              </span>
            </div>
            <div style={{ marginLeft: 36 }}>{reply.text}</div>
          </li>
        ))}
        {ticket.replies.length === 0 && (
          <li style={{ color: "#888" }}>No hay respuestas aún.</li>
        )}
      </ul>
      {ticket.status !== "Resolved" && (
        <ReplyForm
          ticketId={ticket.id}
          userId={userId}
          onSuccess={onReplyAdded}
        />
      )}
    </div>
  );
}
