import { useEffect, useState } from "react";
import type { Ticket } from "../types/Ticket";
import ReplyForm from "./ReplyForm";
import { useMarkTicketAsResolved } from "../hooks/useMarkTicketAsResolved";
import styles from "./TicketDetail.module.css";

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
    <div className={styles.detailContainer}>
      <div className={styles.header}>
        <div>
          <h2 className={styles.title}>{ticket.subject}</h2>
          <div className={styles.userInfo}>
            <span className={styles.userName}>{ticket.userName}</span>
            {avatarUrl && (
              <img
                src={avatarUrl}
                alt="avatar"
                className={styles.userAvatar}
              />
            )}
          </div>
        </div>
        {ticket.status !== "Resolved" && (
          <button
            className={styles.resolveBtn}
            onClick={handleResolve}
            disabled={loading}
          >
            {loading ? "Resolving..." : "Resolve"}
          </button>
        )}
      </div>
      {error && <div style={{ color: "red", margin: "8px 0" }}>{error}</div>}
      <div className={styles.ticketDesc}>{ticket.description}</div>
      <hr />
      <h4>Replies</h4>
      <div className={styles.repliesBlock}>
        {ticket.replies.length === 0 && (
          <div style={{ color: "#888", marginTop: 8 }}>No replies yet.</div>
        )}
        {ticket.replies.map(reply => (
          <div key={reply.id} className={styles.replyCard}>
            <div className={styles.replyAvatar}>
              {reply.avatar ? (
                <img
                  src={`data:image/png;base64,${reply.avatar}`}
                  alt="avatar"
                  style={{ width: 32, height: 32, borderRadius: "50%" }}
                />
              ) : (
                <svg viewBox="0 0 24 24" width={24} height={24} fill="#bfc9d7">
                  <circle cx="12" cy="8" r="4" />
                  <path d="M12 14c-4 0-6 2-6 3.5V20h12v-2.5c0-1.5-2-3.5-6-3.5z" />
                </svg>
              )}
            </div>
            <div className={styles.replyBody}>
              <div className={styles.replyMeta}>
                <span className={styles.replyName}>{reply.userName}</span>
                <span>{new Date(reply.createdAt).toLocaleString()}</span>
              </div>
              <div className={styles.replyText}>{reply.text}</div>
            </div>
          </div>
        ))}
      </div>
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
