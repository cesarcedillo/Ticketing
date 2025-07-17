export type UserType = "Customer" | "Agent" | "Admin";

export interface User {
  id: string;
  userName: string;
  avatar: string;
  type: UserType;
}
