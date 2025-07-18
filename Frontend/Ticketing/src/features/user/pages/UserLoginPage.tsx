import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useUserLogin } from "../hooks/useUserLogin";
import UserLoginForm from "../components/UserLoginForm/UserLoginForm";

export default function UserLoginPage() {
  const navigate = useNavigate();
  const { user, error, loading, login } = useUserLogin();

  const handleLogin = async (username: string) => {
    await login(username);
  };

  useEffect(() => {
    if (user) {
      navigate(`/ticketing`, { state: { user } });
    }
  }, [user, navigate]);

  return (
    <div
      style={{
        minHeight: "100vh",
        background: "#f1f5f9",
        display: "flex",
        alignItems: "center",
        justifyContent: "center"
      }}
    >
      <UserLoginForm
        onLogin={handleLogin}
        loading={loading}
        error={error}
      />
    </div>
  );
}
