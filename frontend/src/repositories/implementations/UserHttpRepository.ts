import { enqueueSnackbar } from "notistack";
import { User } from "../../objects/Entities";
import IUserHttpRepository from "../interfaces/IUserHttpRepository";
import { handleCommonRequest, handleCommonResponse } from "./RequestHandler";

export default class UserHttpRepository implements IUserHttpRepository {
  private baseURL: string;

  constructor(baseURL: string) {
    this.baseURL = baseURL;
  }

  async signinAsync(
    email: string,
    password: string
  ): Promise<User | undefined> {
    try {
      const user = await handleCommonRequest<User>(
        `${this.baseURL}api/user/sign-in`,
        "POST",
        { email, password }
      );

      enqueueSnackbar("Вы успешно авторизовались", {
        variant: "success",
      });

      return user;
    } catch (error) {
      enqueueSnackbar("Не удалось авторизоваться, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async getAsync(): Promise<User[] | undefined> {
    try {
      const response = await fetch(`${this.baseURL}api/user/get-list`, {
        credentials: "include",
      });

      if (!handleCommonResponse(response)) return;

      return await response.json();
    } catch (error) {
      enqueueSnackbar("Не удалось получить пользователей, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async getByIdAsync(id: string): Promise<User | undefined> {
    try {
      const response = await fetch(`${this.baseURL}api/user/get-by-id/${id}`, {
        credentials: "include",
      });

      if (!handleCommonResponse(response)) return;

      return await response.json();
    } catch (error) {
      enqueueSnackbar("Не удалось получить пользователей, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async postAsync(entity: User): Promise<void> {
    try {
      await handleCommonRequest<void>(
        `${this.baseURL}api/user/sign-up`,
        "POST",
        entity
      );

      enqueueSnackbar("Вы успешно зарегистрировались, нужно авторизоваться", {
        variant: "success",
      });
    } catch (error) {
      enqueueSnackbar("Не удалось зарегистрироваться, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async putAsync(entity: User): Promise<void> {
    try {
      await handleCommonRequest<void>(
        `${this.baseURL}api/user/update`,
        "PUT",
        entity
      );

      enqueueSnackbar("Информация успешно обновлена", { variant: "success" });
    } catch (error) {
      enqueueSnackbar("Пользователь не был изменен, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async deleteAsync(id: string): Promise<void> {
    try {
      await handleCommonRequest<void>(
        `${this.baseURL}api/user/delete/${id}`,
        "DELETE",
        {}
      );

      enqueueSnackbar("Профиль удален", { variant: "success" });
    } catch (error) {
      enqueueSnackbar("Пользователь не был удален, попробуйте снова", {
        variant: "error",
      });
    }
  }
}
