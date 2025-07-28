import type { User } from "../../user/types/User";


export interface TicketSummary {
  id: string;
  subject: string;
  status: string;
  user: User;
}

export interface TicketReply {
  id: string;
  text: string;
  createdAt: string;
  user: User;
}

export interface Ticket extends TicketSummary {
  description: string;
  replies: TicketReply[];
}
