import { useContext, useEffect } from "react";
import AuthorizationCard from "../../components/cards/authorizationCard/AuthorizationCard";

import "./AuthorizationPage.css";
import { UserAuthorizationContext } from "../../contexts/UserAuthorizationContext";
import { useNavigate } from "react-router";

export default function AuthorizationPage() {
  const { user } = useContext(UserAuthorizationContext);
  const navigate = useNavigate();

  useEffect(() => {
    if (user) navigate("/");
  }, [user]);

  return (
    <div>
      <div className="main">
        <AuthorizationCard />
      </div>
    </div>
  );
}
