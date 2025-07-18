import styles from "./TicketList.module.css";
import type { TicketSummary } from "../../types/Ticket";

type Props = {
  tickets: TicketSummary[];
  selectedId: string | null;
  onSelect: (id: string) => void;
};

export default function TicketList({ tickets, selectedId, onSelect }: Props) {
  return (
    <div className={styles.listContainer}>
      {tickets.map(ticket => (
        <div
          key={ticket.id}
          className={`${styles.ticketItem} ${ticket.id === selectedId ? styles.selected : ""}`}
          onClick={() => onSelect(ticket.id)}
        >
          {ticket.avatar ? (
            <img
              src={`data:image/png;base64,${ticket.avatar}`}
              className={styles.avatar}
              alt="avatar"
            />
          ) : (
            <div className={styles.avatar}>
              {/* Icono SVG de usuario */}
              <svg viewBox="0 0 24 24" width={20} height={20} fill="currentColor">
                <circle cx="12" cy="8" r="4" />
                <path d="M12 14c-4 0-6 2-6 3.5V20h12v-2.5c0-1.5-2-3.5-6-3.5z" />
              </svg>
            </div>
          )}
          <div className={styles.ticketInfo}>
            <span className={styles.ticketSubject}>{ticket.subject}</span>
            <span className={styles.ticketUser}>
              {ticket.userName} • {ticket.id.slice(0, 6)}
            </span>
          </div>
        </div>
      ))}
    </div>
  );
}
