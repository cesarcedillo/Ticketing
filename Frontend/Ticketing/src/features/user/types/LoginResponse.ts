import type { User } from "./User";

export interface LoginResponse {
  success: boolean;
  message: string;
  accessToken?: string;
  expiration?: string;
  user?: User;
};
