import { useState, useEffect } from "react";
import { useTicketSummaries } from "../hooks/useTicketSummaries";
import { useTicketDetail } from "../hooks/useTicketDetail";
import TicketList from "../components/TicketList/TicketList";
import TicketDetail from "../components/TicketDetail/TicketDetail";
import NewTicketModal from "../components/NewTicketModal/NewTicketModal";
import type { User } from "../../user/types/User";
import { useLocation } from "react-router-dom";

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
    <div style={{ display: "flex", height: "100vh" }}>
      <div style={{ width: "33%", borderRight: "1px solid #eee", padding: "1.5rem 1rem 1.5rem 1.5rem", background: "#f5f7fa" }}>
        <div style={{ marginBottom: 16, display: "flex", justifyContent: "space-between" }}>
          <h3 style={{ margin: 0, color: "#317aff" }}>Tickets</h3>
          <button onClick={() => setShowNewModal(true)} style={{ background: "#fff", color: "#317aff", border: "1px solid #317aff", borderRadius: 5, padding: "4px 12px", fontWeight: 500, cursor: "pointer" }}>
            New Ticket
          </button>
        </div>
        <TicketList
          tickets={tickets}
          selectedId={selectedId}
          onSelect={setSelectedId}
        />
        {loadingList && <p>Cargando tickets...</p>}
        {errorList && <p style={{ color: "red" }}>{errorList}</p>}
        {showNewModal && user?.id && (
          <NewTicketModal
            userId={user.id}
            onClose={() => setShowNewModal(false)}
            onCreated={handleTicketCreated}
          />
        )}
      </div>
      <div style={{ width: "67%", padding: "2rem" }}>
        {loadingDetail && <p>Cargando detalle...</p>}
        {errorDetail && <p style={{ color: "red" }}>{errorDetail}</p>}
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
