import { useState, useEffect } from "react";
import { useTicketSummaries } from "../../hooks/useTicketSummaries";
import { useTicketDetail } from "../../hooks/useTicketDetail";
import TicketList from "../../components/TicketList/TicketList";
import TicketDetail from "../../components/TicketDetail/TicketDetail";
import NewTicketModal from "../../components/NewTicketModal/NewTicketModal";
import type { User } from "../../../user/types/User";
import { useLocation } from "react-router-dom";
import styles from "./TicketingPage.module.css";

export default function TicketingPage() {
  const location = useLocation();
  const user = (location.state as { user: User })?.user;

  const { tickets, loading: loadingList, error: errorList, refetch: refetchTickets } = useTicketSummaries();

  const {
    ticket,
    loading: loadingDetail,
    error: errorDetail,
    loadTicket
  } = useTicketDetail();

  const [selectedId, setSelectedId] = useState<string | null>(null);
  const [showNewModal, setShowNewModal] = useState(false);

  useEffect(() => {
    if (selectedId) {
      loadTicket(selectedId);
    }
  }, [selectedId, loadTicket]);

  const handleTicketCreated = () => {
    refetchTickets();
  };

  const handleReplyOrResolved = () => {
    if (selectedId) {
      loadTicket(selectedId);
      refetchTickets();
    }
  };

  return (
    <div className={styles.container}>
      <div className={styles.leftPanel}>
        <div className={styles.header}>
          <h3 className={styles.title}>Tickets</h3>
          <button
            className={styles.newTicketBtn}
            onClick={() => setShowNewModal(true)}
          >
            New Ticket
          </button>
        </div>
        <TicketList
          tickets={tickets}
          selectedId={selectedId}
          onSelect={setSelectedId}
        />
        {loadingList && <p className={styles.statusMsg}>Cargando tickets...</p>}
        {errorList && <p className={styles.errorMsg}>{errorList}</p>}
        {showNewModal && user?.id && (
          <NewTicketModal
            userId={user.id}
            onClose={() => setShowNewModal(false)}
            onCreated={handleTicketCreated}
          />
        )}
      </div>
      <div className={styles.rightPanel}>
        {loadingDetail && <p className={styles.statusMsg}>Cargando detalle...</p>}
        {errorDetail && <p className={styles.errorMsg}>{errorDetail}</p>}
        <TicketDetail
          ticket={ticket}
          userId={user?.id ?? ""}
          onReplyAdded={handleReplyOrResolved}
          onResolved={handleReplyOrResolved}
        />
      </div>
    </div>
  );
}
