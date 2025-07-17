import { useState } from "react";
import { useNavigate } from "react-router-dom";

type User = {
  userName: string;
  avatar: number[]; // Binario (array de bytes)
  type: "Customer" | "Agent" | "Admin";
};

export default function HomePage() {
  const [username, setUsername] = useState<string>("");
  const [error, setError] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();

  const handleSearch = async () => {
    setError("");
    if (!username) {
      setError("Please enter a username.");
      return;
    }
    setLoading(true);
    try {
      const response = await fetch(`https://localhost:7086/api/User/${username}`);
      if (response.status === 404) {
        setError("The username is not valid");
      } else if (response.ok) {
        const user: User = await response.json();
        navigate(`/user/${encodeURIComponent(user.userName)}`, { state: { user } });
      } else {
        setError("Unexpected error");
      }
    } catch {
      setError("Error connecting to the API");
    }
    setLoading(false);
  };

  const handleKeyDown = (e: React.KeyboardEvent<HTMLInputElement>) => {
    if (e.key === "Enter") handleSearch();
  };

  return (
    <div style={{ padding: "2rem" }}>
      <h2>Ticketing Login</h2>
      <input
        type="text"
        placeholder="Enter username"
        value={username}
        onChange={e => setUsername(e.target.value)}
        onKeyDown={handleKeyDown}
      />
      <button onClick={handleSearch} style={{ marginLeft: 8 }} disabled={loading}>
        {loading ? "Loading..." : "Search"}
      </button>
      {error && <p style={{ color: "red" }}>{error}</p>}
    </div>
  );
}
