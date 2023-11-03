import { enqueueSnackbar } from "notistack";
import { Lot } from "../../objects/Entities";
import ILotHttpRepository from "../interfaces/ILotHttpRepository";
import { handleCommonRequest, handleCommonResponse } from "./RequestHandler";
import { State } from "../../objects/Enums";

export default class LotHttpRepository implements ILotHttpRepository {
  private baseURL: string;

  constructor(baseURL: string) {
    this.baseURL = baseURL;
  }

  async getAsync(): Promise<Lot[] | undefined> {
    try {
      const response = await fetch(`${this.baseURL}api/lot/get-list`, {
        credentials: "include",
      });

      if (!handleCommonResponse(response)) return;

      return await response.json();
    } catch (error) {
      enqueueSnackbar("Не удалось получить лоты, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async getByAuctionAsync(auctionId: string): Promise<Lot[] | undefined> {
    try {
      const response = await fetch(
        `${this.baseURL}api/lot/get-list-by-auction/${auctionId}`,
        {
          credentials: "include",
        }
      );

      if (!handleCommonResponse(response)) return;

      return await response.json();
    } catch (error) {
      enqueueSnackbar(
        "Не удалось получить лоты по аукциону, попробуйте снова",
        { variant: "error" }
      );
    }
  }

  async postAsync(entity: Lot): Promise<void> {
    throw new Error("Метод не реализован");
  }

  async createLotAsync(formData: FormData): Promise<void> {
    try {
      const response = await fetch(`${this.baseURL}api/lot/create`, {
        method: "POST",
        body: formData,
        credentials: "include",
      });

      if (!handleCommonResponse(response)) return;

      enqueueSnackbar("Лот успешно создан", { variant: "success" });
    } catch (error) {
      enqueueSnackbar("Не удалось создать лот, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async putAsync(entity: Lot): Promise<void> {
    try {
      await handleCommonRequest(`${this.baseURL}api/lot/update`, "PUT", entity);

      enqueueSnackbar("Лот успешно изменен", { variant: "success" });
    } catch (error) {
      enqueueSnackbar("Не удалось изменить лот, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async deleteAsync(id: string): Promise<void> {
    try {
      await handleCommonRequest(
        `${this.baseURL}api/lot/delete/${id}`,
        "DELETE",
        {}
      );

      enqueueSnackbar("Лот успешно удален", { variant: "success" });
    } catch (error) {
      enqueueSnackbar("Не удалось удалить лот, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async changeStateAsync(
    auctionId: string,
    lotId: string,
    state: State
  ): Promise<void> {
    try {
      await handleCommonRequest(`${this.baseURL}api/lot/change-status`, "PUT", {
        auctionId,
        lotId,
        state,
      });

      enqueueSnackbar("Статус лота успешно обновлен", {
        variant: "success",
      });
    } catch (error) {
      enqueueSnackbar("Не удалось изменить статус лота, попробуйте снова", {
        variant: "error",
      });
    }
  }

  async doBetAsync(
    auctionId: string,
    lotId: string,
    userId: string
  ): Promise<void> {
    try {
      await handleCommonRequest(`${this.baseURL}api/lot/do-bet`, "PUT", {
        auctionId,
        lotId,
        userId,
      });

      enqueueSnackbar("Ставка сделана успешно", {
        variant: "success",
      });
    } catch (error) {
      enqueueSnackbar("Не удалось сделать ставку, попробуйте снова", {
        variant: "error",
      });
    }
  }
}
