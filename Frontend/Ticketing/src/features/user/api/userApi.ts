import type { User } from "../types/User";
import { API_BASE_URL } from "../../../config/apiConfig";

export async function fetchUser(username: string): Promise<User> {
  const response = await fetch(`${API_BASE_URL}/api/User/${username}`);
  if (!response.ok) throw response;
  return response.json();
}
