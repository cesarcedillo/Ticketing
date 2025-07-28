import { useState } from "react";
import { loginUser } from "../api/userApi";
import type { User } from "../types/User";

export function useUserLogin() {
  const [user, setUser] = useState<User | null>(null);
  const [error, setError] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);

  const login = async (username: string, password: string) => {
    setLoading(true);
    setError(null);
    try {
      const result = await loginUser(username, password);

      if (!result.success) {
        setError(result.message || "Invalid username or password");
        setLoading(false);
        return;
      }

      setUser(result.user!);
      localStorage.setItem("token", result.accessToken!);
    } catch (e: any) {
      setError(e.message || "Login failed");
    }
    setLoading(false);
  };

  return { user, error, loading, login };
}
