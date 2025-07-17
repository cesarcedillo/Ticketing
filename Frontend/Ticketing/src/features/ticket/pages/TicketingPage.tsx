//import { useLocation } from "react-router-dom";
import { useState, useEffect } from "react";
import { useTicketSummaries } from "../hooks/useTicketSummaries";
import { useTicketDetail } from "../hooks/useTicketDetail";
import TicketList from "../components/TicketList";
import TicketDetail from "../components/TicketDetail";
//import type { User } from "../../user/types/User";

export default function TicketingPage() {
  //const location = useLocation();
  //const user = (location.state as { user: User })?.user;
  const { tickets, loading: loadingList, error: errorList } = useTicketSummaries();
  const { ticket, loading: loadingDetail, error: errorDetail, loadTicket } = useTicketDetail();
  const [selectedId, setSelectedId] = useState<string | null>(null);

  useEffect(() => {
    if (selectedId) {
      loadTicket(selectedId);
    }
  }, [selectedId, loadTicket]);

  return (
    <div style={{ display: "flex", height: "100vh" }}>
      <div style={{ width: "20%", borderRight: "1px solid #eee", padding: "1.5rem" }}>
        <TicketList
          tickets={tickets}
          selectedId={selectedId}
          onSelect={setSelectedId}
        />
        {loadingList && <p>Cargando tickets...</p>}
        {errorList && <p style={{ color: "red" }}>{errorList}</p>}
      </div>
      <div style={{ width: "80%", padding: "2rem" }}>
        {loadingDetail ? <p>Cargando detalle...</p> : <TicketDetail ticket={ticket} />}
        {errorDetail && <p style={{ color: "red" }}>{errorDetail}</p>}
      </div>
    </div>
  );
}
