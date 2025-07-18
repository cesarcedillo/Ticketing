import type { Ticket, TicketSummary } from "../types/Ticket";
import { API_BASE_URL } from "../../../config/apiConfig";

export async function fetchTicketSummaries(): Promise<TicketSummary[]> {
  const resp = await fetch(`${API_BASE_URL}/api/Ticket`);
  if (!resp.ok) throw new Error("Error loading tickets");
  return resp.json();
}

export async function fetchTicketDetail(id: string): Promise<Ticket> {
  const resp = await fetch(`${API_BASE_URL}/api/Ticket/${id}`);
  if (!resp.ok) throw new Error("Error loading ticket detail");
  return resp.json();
}

export async function postTicketReply(ticketId: string, text: string, userId: string): Promise<void> {
  const resp = await fetch(`${API_BASE_URL}/api/Ticket/${ticketId}/replies`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ text, userId })
  });
  if (!resp.ok) {
    throw new Error("The reply could not be sent");
  }
}

export async function createTicket(subject: string, description: string, userId: string): Promise<void> {
  const resp = await fetch(`${API_BASE_URL}/api/Ticket`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify({ subject, description, userId })
  });
  if (!resp.ok) {
    throw new Error("The ticket could not be created");
  }
}

export async function markTicketAsResolved(ticketId: string): Promise<void> {
  const resp = await fetch(`${API_BASE_URL}/api/Ticket/${ticketId}/mark-as-resolved`, {
    method: "PATCH",
  });
  if (!resp.ok) {
    throw new Error("the ticket could not be marked as resolved");
  }
}