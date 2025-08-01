import { API_BASE_URL } from "../../../config/apiConfig";
import type { LoginResponse } from "../types/LoginResponse";

export async function loginUser(
  username: string,
  password: string
): Promise<LoginResponse> {
  const response = await fetch(`${API_BASE_URL}/api/User/signin`, {
    method: "POST",
    headers: {
      accept: "application/json",
      "Content-Type": "application/json",
    },
    body: JSON.stringify({ username, password }),
  });

  if (!response.ok) {
    const error = await response.text();
    throw new Error(error || "Login failed");
  }

  return response.json();
}
