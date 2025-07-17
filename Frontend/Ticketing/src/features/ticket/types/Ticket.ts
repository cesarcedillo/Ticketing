export interface TicketSummary {
  id: string;
  subject: string;
  status: string;
  userName: string;
  userId: string;
  avatar: string;
}

export interface TicketReply {
  id: string;
  text: string;
  createdAt: string;
  userId: string;
  userName: string;
  avatar: string;
}

export interface Ticket extends TicketSummary {
  description: string;
  replies: TicketReply[];
}
