import { useState } from "react";
import { useCreateTicket } from "../hooks/useCreateTicket";

type Props = {
  userId: string;
  onClose: () => void;
  onCreated: () => void;
};

export default function NewTicketModal({ userId, onClose, onCreated }: Props) {
  const [subject, setSubject] = useState("");
  const [description, setDescription] = useState("");
  const { loading, error, create } = useCreateTicket();

  const handleCreate = async () => {
    if (!subject.trim() || !description.trim()) return;
    try {
      await create(subject, description, userId);
      setSubject("");
      setDescription("");
      onCreated();
      onClose();
    } catch {
    }
  };

  return (
    <div
      style={{
        position: "fixed", top: 0, left: 0, width: "100vw", height: "100vh",
        background: "rgba(0,0,0,0.35)", display: "flex", alignItems: "center", justifyContent: "center", zIndex: 1000
      }}>
      <div style={{ background: "white", borderRadius: 8, padding: 24, minWidth: 320, boxShadow: "0 2px 16px #3332" }}>
        <h3>New Ticket</h3>
        <label>
          Subject<br />
          <input
            style={{ width: "100%", marginBottom: 8 }}
            value={subject}
            onChange={e => setSubject(e.target.value)}
            disabled={loading}
          />
        </label>
        <label>
          Description<br />
          <textarea
            style={{ width: "100%", marginBottom: 8 }}
            rows={4}
            value={description}
            onChange={e => setDescription(e.target.value)}
            disabled={loading}
          />
        </label>
        <div>
          <button onClick={handleCreate} disabled={loading || !subject.trim() || !description.trim()}>
            {loading ? "Creating..." : "Create"}
          </button>
          <button onClick={onClose} style={{ marginLeft: 8 }} disabled={loading}>
            Cancel
          </button>
        </div>
        {error && <div style={{ color: "red", marginTop: 8 }}>{error}</div>}
      </div>
    </div>
  );
}
