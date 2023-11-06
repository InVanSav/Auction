import {
  createContext,
  useEffect,
  useState,
  PropsWithChildren,
  useContext,
} from "react";

import { User } from "../objects/Entities";

import UserHttpRepository from "../repositories/implementations/UserHttpRepository";
import { useLocation, useNavigate } from "react-router-dom";
import { AuctionContext } from "./AuctionContext";
import { useLocalStorage } from "@uidotdev/usehooks";

export interface IUserAuthorizationContext {
  user: User | undefined;
  members: User[] | undefined;

  signup: (login: string, email: string, password: string) => void;
  signin: (email: string, password: string) => void;
  signout: () => void;

  reloadUserData: () => void;
}

export const UserAuthorizationContext =
  createContext<IUserAuthorizationContext>({} as IUserAuthorizationContext);

export const UserAuthorizationProvider: React.FC<PropsWithChildren> = ({
  children,
}) => {
  const userHttpRepository = new UserHttpRepository("https://localhost:7132/");

  const [activeUserId, saveActiveUserId] = useLocalStorage(
    "savedActiveUserId",
    ""
  );

  const navigate = useNavigate();
  const location = useLocation();

  const [user, setUser] = useState<User | undefined>(undefined);
  const [members, setMembers] = useState<User[] | undefined>(undefined);

  const { auction } = useContext(AuctionContext);

  useEffect(() => {
    reloadUserData();
  }, []);

  useEffect(() => {
    const fetchMembers = async () => {
      if (!user) return;
      setMembers(await userHttpRepository.getAsync());
    };

    fetchMembers();
  }, [user]);

  const signup = async (login: string, email: string, password: string) => {
    const user: User = {
      id: "5D9871B2-C2E4-4DAF-945F-A1E78F8724EC",
      name: login,
      email: email,
      password: password,
    };

    await userHttpRepository.postAsync(user);

    navigate("/authorization");
  };

  const signin = async (email: string, password: string) => {
    const user = await userHttpRepository.signinAsync(email, password);

    if (!user) return;

    setUser(user);
    saveActiveUserId(user.id);

    navigate("/");
  };

  const signout = () => {
    localStorage.clear();
    setUser(undefined);

    userHttpRepository.signoutAsync();

    navigate("/authorization");
  };

  const reloadUserData = async () => {
    if (!activeUserId) {
      navigate("/authorization");
      return;
    }

    const user = await userHttpRepository.getByIdAsync(activeUserId);

    if (!user) {
      navigate("/authorization");
      return;
    }

    navigate(location);
    setUser(user!);
  };

  return (
    <UserAuthorizationContext.Provider
      value={{
        user,
        signup,
        signin,
        signout,
        reloadUserData,
        members,
      }}
    >
      {children}
    </UserAuthorizationContext.Provider>
  );
};
