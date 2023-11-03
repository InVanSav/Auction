import { Lot } from "../../objects/Entities";
import { State } from "../../objects/Enums";
import IBaseHttpRepository from "./IBaseHttpRepository";

export default interface ILotHttpRepository extends IBaseHttpRepository<Lot> {
  getByAuctionAsync(auctionId: string): Promise<Lot[] | undefined>;

  createLotAsync(formData: FormData): Promise<void>;

  doBetAsync(auctionId: string, lotId: string, userId: string): Promise<void>;

  changeStateAsync(
    auctionId: string,
    lotId: string,
    state: State
  ): Promise<void>;
}
