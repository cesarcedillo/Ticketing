import { useState } from "react";
import { useCreateTicket } from "../hooks/useCreateTicket";
import styles from "./NewTicketModal.module.css";

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
      // error handled by hook
    }
  };

  return (
    <div className={styles.modalOverlay}>
      <div className={styles.modalBox}>
        <div className={styles.title}>New Ticket</div>
        <form
          className={styles.formGroup}
          onSubmit={e => { e.preventDefault(); handleCreate(); }}
        >
          <div>
            <div className={styles.label}>Subject</div>
            <input
              className={styles.input}
              value={subject}
              onChange={e => setSubject(e.target.value)}
              disabled={loading}
              maxLength={80}
              autoFocus
              placeholder="Enter a short title..."
            />
          </div>
          <div>
            <div className={styles.label}>Description</div>
            <textarea
              className={styles.textarea}
              value={description}
              onChange={e => setDescription(e.target.value)}
              disabled={loading}
              maxLength={800}
              placeholder="Describe your issue or request..."
            />
          </div>
          <div className={styles.buttonRow}>
            <button
              className={styles.cancelBtn}
              onClick={onClose}
              disabled={loading}
              type="button"
            >
              Cancel
            </button>
            <button
              className={styles.createBtn}
              disabled={loading || !subject.trim() || !description.trim()}
              type="submit"
            >
              {loading ? "Creating..." : "Create"}
            </button>
          </div>
          {error && <div className={styles.errorMsg}>{error}</div>}
        </form>
      </div>
    </div>
  );
}
