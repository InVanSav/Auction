import { Auction } from "../../objects/Entities";
import { State } from "../../objects/Enums";
import IBaseHttpRepository from "./IBaseHttpRepository";

export default interface IAuctionHttpRepository
  extends IBaseHttpRepository<Auction> {
  getByIdAsync(id: string): Promise<Auction | undefined>;

  setDateStartAsync(id: string): Promise<void>;

  setDateEndAsync(id: string): Promise<void>;

  changeStateAsync(id: string, state: State): Promise<void>;
}
