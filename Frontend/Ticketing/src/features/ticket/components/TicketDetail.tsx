import { useEffect, useState } from "react";
import type { Ticket } from "../types/Ticket";
import { bytesToImageUrl } from "../../../shared/utils/bytesToImageUrl";

type Props = {
  ticket: Ticket | undefined;
};

export default function TicketDetail({ ticket }: Props) {
  const [avatarUrl, setAvatarUrl] = useState<string>();
  const [repliesAvatars, setRepliesAvatars] = useState<Record<string, string>>({});

  useEffect(() => {
    if (ticket?.avatar && ticket.avatar.length > 0) {
      const url = bytesToImageUrl(ticket.avatar);
      setAvatarUrl(url!);
      return () => { if (url) URL.revokeObjectURL(url); };
    } else {
      setAvatarUrl(undefined);
    }
  }, [ticket]);

  useEffect(() => {
    if (ticket?.replies) {
      const newAvatars: Record<string, string> = {};
      ticket.replies.forEach(reply => {
        if (reply.avatar && reply.avatar.length > 0) {
          const url = bytesToImageUrl(reply.avatar);
          if (url) newAvatars[reply.id] = url;
        }
      });
      setRepliesAvatars(newAvatars);

      return () => {
        Object.values(newAvatars).forEach(url => URL.revokeObjectURL(url));
      };
    }
  }, [ticket]);

  if (!ticket) return <div>Selecciona un ticket de la lista</div>;

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
        <span style={{ color: "#666", fontSize: 13 }}>({ticket.status})</span>
      </div>
      <p style={{ marginTop: 16 }}>{ticket.description}</p>

      <hr />
      <h4>Respuestas</h4>
      <ul style={{ listStyle: "none", padding: 0 }}>
        {ticket.replies.map(reply => (
          <li key={reply.id} style={{ marginBottom: 18 }}>
            <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
              {repliesAvatars[reply.id] && (
                <img
                  src={repliesAvatars[reply.id]}
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
      </ul>
    </div>
  );
}
