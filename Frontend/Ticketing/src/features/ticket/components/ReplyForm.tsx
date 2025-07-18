import { useState } from "react";
import { usePostTicketReply } from "../hooks/usePostTicketReply";
import styles from "./ReplyForm.module.css";

type Props = {
  ticketId: string;
  userId: string;
  onSuccess: () => void;
};

export default function ReplyForm({ ticketId, userId, onSuccess }: Props) {
  const [text, setText] = useState("");
  const { loading, error, sendReply } = usePostTicketReply();

  const handleSend = async () => {
    if (!text.trim()) return;
    try {
      await sendReply(ticketId, text, userId);
      setText("");
      onSuccess();
    } catch {
      // error handled by hook
    }
  };

  return (
    <div className={styles.replyFormBox}>
      <textarea
        rows={3}
        className={styles.replyTextarea}
        placeholder="Write your reply..."
        value={text}
        onChange={e => setText(e.target.value)}
        disabled={loading}
        maxLength={700}
      />
      <div className={styles.actionRow}>
        <button
          className={styles.sendBtn}
          onClick={handleSend}
          disabled={loading || !text.trim()}
        >
          {loading ? "Sending..." : "Send"}
        </button>
        {error && <span className={styles.errorMsg}>{error}</span>}
      </div>
    </div>
  );
}
