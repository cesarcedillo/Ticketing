import type { Ticket } from "../types/Ticket";

type Props = {
  ticket: Ticket | undefined;
};

export default function TicketDetail({ ticket }: Props) {
  if (!ticket) return <div>Selecciona un ticket de la lista</div>;

  return (
    <div>
      <h2>{ticket.subject}</h2>
      <div style={{ display: "flex", alignItems: "center", gap: 8 }}>
        {ticket.avatar && (
          <img
            src={`data:image/png;base64,${ticket.avatar}`}
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
              {reply.avatar && (
                <img
                  src={`data:image/png;base64,${reply.avatar}`}
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
