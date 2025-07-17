import type { User } from "../types/User";

export async function fetchUser(username: string): Promise<User> {
  const response = await fetch(`https://localhost:7086/api/User/${username}`);
  if (!response.ok) throw response;
  return response.json();
}
