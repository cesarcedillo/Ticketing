import { useState } from "react";
import { fetchUser } from "../api/userApi";
import type { User } from "../types/User";

export function useUserLogin() {
  const [user, setUser] = useState<User | null>(null);
  const [error, setError] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);

  async function login(username: string) {
    setLoading(true);
    setError("");
    try {
      const u = await fetchUser(username);
      setUser(u);
    } catch (err: any) {
      if (err.status === 404) setError("The username is not valid");
      else setError("Unexpected error");
      setUser(null);
    }
    setLoading(false);
  }

  return { user, error, loading, login };
}
