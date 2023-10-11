import { useState } from "react";

import "./AuthorityCard.css";
import { useUserAuthorityContext } from "../../../contexts/UserAuthorityContext";

export default function AuthorityCard() {
  const userAuthorityContext = useUserAuthorityContext();

  const [isSignin, setIsSignin] = useState<boolean>(true);
  const [error, setError] = useState<string | null>(null);

  const [login, setLogin] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [repeatPassword, setRepeatPassword] = useState<string>("");

  const validateField = (
    value: string,
    validationRegex: RegExp,
    errorMessage: string
  ): boolean => {
    if (!value) {
      setError("Все поля должны быть заполнены!");
      return false;
    }

    if (!validationRegex?.test(value)) {
      setError(errorMessage);
      return false;
    }

    return true;
  };

  const repeatPasswordValidation = (pwd1: string, pwd2: string): boolean => {
    if (!pwd1 || !pwd2) {
      setError("Пароли должны быть указаны!");
      return false;
    }

    if (pwd1 !== pwd2) {
      setError("Пароли не совпадают!");
      return false;
    }

    return true;
  };

  const commonValidation = (email: string, password: string): boolean => {
    if (
      !validateField(
        email,
        /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i,
        "Введите корректный email"
      )
    )
      return false;

    if (
      !validateField(
        password,
        /^\S{8,30}$/m,
        "Пароль должен быть не короче 8 символов"
      )
    )
      return false;

    return true;
  };

  const signupValidation = (
    login: string,
    email: string,
    password: string,
    repeatPassword: string
  ): boolean => {
    if (
      !validateField(
        login,
        /^\S{8,30}$/m,
        "Логин не должен содержать специальных знаков!"
      )
    )
      return false;

    if (!commonValidation(email, password)) return false;

    if (!repeatPasswordValidation(password, repeatPassword)) return false;

    return true;
  };

  const signin = () => {
    if (!commonValidation(email, password)) return;

    userAuthorityContext?.signin(email, password);
  };

  const signup = () => {
    if (!signupValidation(login, email, password, repeatPassword)) return;

    userAuthorityContext?.signup(login, email, password);
  };

  return (
    <div className="authority_container">
      <div className="container_buttons">
        <button
          className={`type ${isSignin ? "active" : ""}`}
          onClick={() => setIsSignin(true)}
        >
          Авторизация
        </button>
        <button
          className={`type ${!isSignin ? "active" : ""}`}
          onClick={() => setIsSignin(false)}
        >
          Регистрация
        </button>
      </div>
      {isSignin ? (
        <div className="container_inputs">
          <input
            className="input_item"
            type="email"
            name="email"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            placeholder="example@mail.ru"
          ></input>
          <input
            className="input_item"
            type="password"
            name="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            placeholder="Введите пароль"
          ></input>
          <button
            className="authority_button"
            type="submit"
            onClick={() => signin()}
          >
            Подтвердить
          </button>
        </div>
      ) : (
        <div className="container_inputs">
          <input
            className="input_item"
            type="text"
            name="login"
            value={login}
            onChange={(event) => setLogin(event.target.value)}
            placeholder="Введите логин"
          ></input>
          <input
            className="input_item"
            type="email"
            name="email"
            value={email}
            onChange={(event) => setEmail(event.target.value)}
            placeholder="example@mail.ru"
          ></input>
          <input
            className="input_item"
            type="password"
            name="password"
            value={password}
            onChange={(event) => setPassword(event.target.value)}
            placeholder="Введите пароль"
          ></input>
          <input
            className="input_item"
            type="password"
            name="repeat_password"
            value={repeatPassword}
            onChange={(event) => setRepeatPassword(event.target.value)}
            placeholder="Повторите пароль"
          ></input>
          <button
            className="authority_button"
            type="submit"
            onClick={() => signup()}
          >
            Подтвердить
          </button>
        </div>
      )}
      <div className="error">{error}</div>
    </div>
  );
}
