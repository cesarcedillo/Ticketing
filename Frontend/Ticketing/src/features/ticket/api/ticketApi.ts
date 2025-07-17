import type { Ticket, TicketSummary } from "../types/Ticket";

// Helper para convertir string binario a array de bytes
function binaryStringToByteArray(binary: string): number[] {
  return Array.from(binary, (c) => c.charCodeAt(0));
}

// --- LISTA DE TICKETS ---
export async function fetchTicketSummaries(): Promise<TicketSummary[]> {
  const resp = await fetch("https://localhost:7086/api/Ticketing");
  if (!resp.ok) throw new Error("Error loading tickets");
  return resp.json();
}

// --- DETALLE DE TICKET ---
export async function fetchTicketDetail(id: string): Promise<Ticket> {
  const resp = await fetch(`https://localhost:7086/api/Ticketing/${id}`);
  if (!resp.ok) throw new Error("Error loading ticket detail");
  return resp.json();
}
