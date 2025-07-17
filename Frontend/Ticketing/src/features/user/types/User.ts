export type UserType = "Customer" | "Agent" | "Admin";

export interface User {
  userName: string;
  avatar: number[];
  type: UserType;
}
