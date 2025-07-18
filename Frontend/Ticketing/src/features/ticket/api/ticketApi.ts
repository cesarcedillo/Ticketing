import type { Ticket, TicketSummary } from "../types/Ticket";

export async function fetchTicketSummaries(): Promise<TicketSummary[]> {
  const resp = await fetch("https://localhost:7086/api/Ticketing");
  if (!resp.ok) throw new Error("Error loading tickets");
  return resp.json();
}

export async function fetchTicketDetail(id: string): Promise<Ticket> {
  const resp = await fetch(`https://localhost:7086/api/Ticketing/${id}`);
  if (!resp.ok) throw new Error("Error loading ticket detail");
  return resp.json();
}

export async function postTicketReply(ticketId: string, text: string, userId: string): Promise<void> {
  const resp = await fetch(`https://localhost:7086/api/Ticketing/${ticketId}/replies`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ text, userId })
  });
  if (!resp.ok) {
    throw new Error("The reply could not be sent");
  }
}

export async function createTicket(subject: string, description: string, userId: string): Promise<void> {
  const resp = await fetch("https://localhost:7086/api/Ticketing", {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ subject, description, userId })
  });
  if (!resp.ok) {
    throw new Error("The ticket could not be created");
  }
}