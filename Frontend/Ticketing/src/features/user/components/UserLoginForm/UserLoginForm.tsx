import { useState } from "react";
import styles from "./UserLoginForm.module.css";

type Props = {
  onLogin: (username: string) => Promise<void>;
  loading?: boolean;
  error?: string | null;
};

export default function UserLoginForm({ onLogin, loading, error }: Props) {
  const [username, setUsername] = useState("");

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (username.trim()) {
      await onLogin(username.trim());
    }
  };

  return (
    <form className={styles.card} onSubmit={handleSubmit}>
      <div className={styles.title}>Support Login</div>
      <div>
        <label className={styles.inputLabel} htmlFor="username">
          Username
        </label>
        <input
          id="username"
          className={styles.inputBox}
          value={username}
          onChange={e => setUsername(e.target.value)}
          placeholder="Enter your username..."
          disabled={loading}
          autoFocus
          autoComplete="username"
        />
      </div>
      <button
        className={styles.button}
        type="submit"
        disabled={loading || !username.trim()}
      >
        {loading ? "Logging in..." : "Login"}
      </button>
      {error && <div className={styles.errorMsg}>{error}</div>}
    </form>
  );
}
