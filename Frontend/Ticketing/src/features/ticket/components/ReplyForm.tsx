import { useState } from "react";
import { usePostTicketReply } from "../hooks/usePostTicketReply";

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
    await sendReply(ticketId, text, userId);
    if (!error) {
      setText("");
      onSuccess();
    }
  };

  return (
    <div style={{ marginTop: 24, background: "#fafbfc", padding: 16, borderRadius: 8 }}>
      <textarea
        rows={3}
        style={{ width: "100%", marginBottom: 8, resize: "vertical" }}
        placeholder="Write here your reply..."
        value={text}
        onChange={e => setText(e.target.value)}
        disabled={loading}
      />
      <div>
        <button onClick={handleSend} disabled={loading || !text.trim()}>
          {loading ? "Sending..." : "Send"}
        </button>
        {error && <span style={{ color: "red", marginLeft: 12 }}>{error}</span>}
      </div>
    </div>
  );
}
