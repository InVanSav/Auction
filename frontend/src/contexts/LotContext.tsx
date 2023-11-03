import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";

import { Lot } from "../objects/Entities";

import LotHttpRepository from "../repositories/implementations/LotHttpRepository";
import { UserAuthorizationContext } from "./UserAuthorizationContext";
import { AuctionContext } from "./AuctionContext";
import { State } from "../objects/Enums";

interface ILotContext {
  lots: Lot[] | undefined;

  getLotsByAuction: () => Promise<Lot[] | undefined>;
  createLot: (formData: FormData) => void;
  deleteLot: (lotId: string) => void;
  changeState: (lotId: string, state: State) => Promise<void>;
  doBet: (lotId: string) => Promise<void>;
}

export const LotContext = createContext<ILotContext>({} as ILotContext);

const lotRepository = new LotHttpRepository("https://localhost:7132/");

export const LotProvider: React.FC<PropsWithChildren> = ({ children }) => {
  const [lots, setLots] = useState<Lot[] | undefined>([]);

  const { user } = useContext(UserAuthorizationContext);
  const { curAuctionId } = useContext(AuctionContext);

  useEffect(() => {
    const fetchLots = async () => {
      if (!user) return;
      setLots(await lotRepository.getAsync());
    };

    fetchLots();
  }, [user]);

  const getLotsByAuction = async (): Promise<Lot[] | undefined> => {
    if (!curAuctionId) return;
    return await lotRepository.getByAuctionAsync(curAuctionId);
  };

  const createLot = async (formData: FormData) => {
    await lotRepository.createLotAsync(formData);
  };

  const deleteLot = async (lotId: string) => {
    await lotRepository.deleteAsync(lotId);
  };

  const changeState = async (lotId: string, state: State) => {
    if (!curAuctionId) return;
    await lotRepository.changeStateAsync(curAuctionId, lotId, state);
  };

  const doBet = async (lotId: string) => {
    if (!curAuctionId) return;
    await lotRepository.doBetAsync(curAuctionId, lotId, user?.id!);
  };

  return (
    <LotContext.Provider
      value={{
        lots,
        getLotsByAuction,
        createLot,
        deleteLot,
        changeState,
        doBet,
      }}
    >
      {children}
    </LotContext.Provider>
  );
};
