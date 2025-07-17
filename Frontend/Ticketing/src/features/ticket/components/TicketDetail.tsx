import { useEffect, useState } from "react";
import type { Ticket } from "../types/Ticket";
import ReplyForm from "./ReplyForm";

type Props = {
  ticket: Ticket | undefined;
  userId: string;                  // El usuario logueado
  onReplyAdded: () => void;        // Función para recargar el detalle tras nueva reply
};

export default function TicketDetail({ ticket, userId, onReplyAdded }: Props) {
  const [avatarUrl, setAvatarUrl] = useState<string | undefined>();

  useEffect(() => {
    if (ticket?.avatar) {
      setAvatarUrl(`data:image/png;base64,${ticket.avatar}`);
    } else {
      setAvatarUrl(undefined);
    }
  }, [ticket]);

  if (!ticket) {
    return <div>Select a ticket from the list</div>;
  }

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
      </div>
      <p style={{ marginTop: 16 }}>{ticket.description}</p>

      <hr />
      <h4>Replies</h4>
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
          <li style={{ color: "#888" }}>There are no reply yet.</li>
        )}
      </ul>

      <ReplyForm
        ticketId={ticket.id}
        userId={userId}
        onSuccess={onReplyAdded}
      />
    </div>
  );
}
