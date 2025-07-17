import { useState } from "react";
import { useNavigate } from "react-router-dom";
import { useUserLogin } from "../hooks/useUserLogin";

export default function UserLoginPage() {
  const [username, setUsername] = useState<string>("");
  const navigate = useNavigate();
  const { user, error, loading, login } = useUserLogin();

  const handleSearch = async () => {
    await login(username);
    if (user) navigate(`/user/${encodeURIComponent(user.userName)}`, { state: { user } });
  };

  return (
    <div style={{ padding: "2rem" }}>
      <h2>Ticketing Login</h2>
      <input
        type="text"
        placeholder="Enter username"
        value={username}
        onChange={e => setUsername(e.target.value)}
        onKeyDown={e => { if (e.key === "Enter") handleSearch(); }}
      />
      <button onClick={handleSearch} disabled={loading} style={{ marginLeft: 8 }}>
        {loading ? "Loading..." : "Search"}
      </button>
      {error && <p style={{ color: "red" }}>{error}</p>}
    </div>
  );
}
