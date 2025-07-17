import { useLocation, useParams } from "react-router-dom";
import type { User } from "../types/User";
import { useEffect, useState } from "react";

export default function UserPage() {
  useParams<{ username: string }>();
  const location = useLocation();
  const user = (location.state as { user?: User })?.user;
  const [avatarUrl, setAvatarUrl] = useState<string>("");

  useEffect(() => {
    if (user?.avatar && user.avatar.length > 0) {
      const byteArray = new Uint8Array(user.avatar);
      const blob = new Blob([byteArray], { type: "image/png" });
      const url = URL.createObjectURL(blob);
      setAvatarUrl(url);
      return () => URL.revokeObjectURL(url);
    }
  }, [user]);

  if (!user) return <div>User not found</div>;

  return (
    <div style={{ padding: "2rem" }}>
      <h2>User found!</h2>
      <p><b>UserName:</b> {user.userName}</p>
      <p><b>User Type:</b> {user.type}</p>
      {avatarUrl && (
        <div>
          <b>Avatar:</b><br />
          <img
            src={avatarUrl}
            alt="avatar"
            style={{ width: 100, height: 100, borderRadius: "50%", border: "1px solid #ccc" }}
          />
        </div>
      )}
    </div>
  );
}
