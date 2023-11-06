import { User } from "../../objects/Entities";
import IBaseHttpRepository from "./IBaseHttpRepository";

export default interface IUserHttpRepository extends IBaseHttpRepository<User> {
  signinAsync(email: string, password: string): Promise<User | undefined>;

  getByIdAsync(id: string): Promise<User | undefined>;

  signoutAsync(): Promise<void>;
}
