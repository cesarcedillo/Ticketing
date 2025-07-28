import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useUserLogin } from "../../hooks/useUserLogin";
import UserLoginForm from "../../components/UserLoginForm/UserLoginForm";
import styles from "./UserLoginPage.module.css";

export default function UserLoginPage() {
  const navigate = useNavigate();
  const { user, error, loading, login } = useUserLogin();

  const handleLogin = async (username: string, password: string) => {
    await login(username, password);
  };

  useEffect(() => {
    if (user) {
      navigate(`/ticketing`, { state: { user } });
    }
  }, [user, navigate]);

  return (
    <div className={styles.bg}>
      <UserLoginForm
        onLogin={handleLogin}
        loading={loading}
        error={error}
      />
    </div>
  );
}
