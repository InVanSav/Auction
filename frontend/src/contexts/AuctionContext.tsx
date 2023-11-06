import {
  PropsWithChildren,
  createContext,
  useContext,
  useEffect,
  useState,
} from "react";

import { Auction, User } from "../objects/Entities";
import { State } from "../objects/Enums";

import AuctionHttpRepository from "../repositories/implementations/AuctionHttpRepository";
import { UserAuthorizationContext } from "./UserAuthorizationContext";
import { useLocalStorage } from "@uidotdev/usehooks";

interface IAuctionContext {
  auctions: Auction[] | undefined;
  auction: Auction | undefined;

  author: User | undefined;
  isAuthor: boolean;

  curAuctionId: string;
  setAuctionId: (auctionId: string) => void;

  createAuction: (
    title: string,
    description: string,
    authorId: string
  ) => Promise<void>;
  getAuction: (id: string) => Promise<Auction | undefined>;
  deleteAuction: (auctionId: string) => Promise<void>;
  changeState: (auctionId: string, state: State) => Promise<void>;
}

export const AuctionContext = createContext<IAuctionContext>(
  {} as IAuctionContext
);

const auctionRepository = new AuctionHttpRepository("https://localhost:7132/");

export const AuctionProvider: React.FC<PropsWithChildren> = ({ children }) => {
  const [auction, setAuction] = useState<Auction>();
  const [auctions, setAuctions] = useState<Auction[] | undefined>([]);
  const { user, members } = useContext(UserAuthorizationContext);

  const [curAuctionId, saveAuctionId] = useLocalStorage("savedAuctionId", "");

  const author = members?.find((member) => member.id === auction?.authorId);
  const isAuthor = user?.id === author?.id;

  useEffect(() => {
    if (!curAuctionId) return;

    const getCurAuctionAsync = async () => {
      setAuction(await getAuction(curAuctionId));
    };

    getCurAuctionAsync();
  }, [curAuctionId]);

  useEffect(() => {
    const fetchAuctions = async () => {
      if (!user) return;
      setAuctions(await auctionRepository.getAsync());
    };

    fetchAuctions();
  }, [user]);

  const createAuction = async (
    title: string,
    description: string,
    authorId: string
  ) => {
    const auction: Auction = {
      id: "B5FEA3BD-F650-4D61-BE5F-0A1411809E4F",
      name: title,
      description: description,
      dateStart: new Date(),
      dateEnd: new Date(),
      authorId: authorId,
      state: State.awaiting,
      lots: [],
    };

    await auctionRepository.postAsync(auction);
  };

  const getAuction = async (
    auctionId: string
  ): Promise<Auction | undefined> => {
    const auction = await auctionRepository.getByIdAsync(auctionId);
    if (!auction) return;
    return auction;
  };

  const deleteAuction = async (auctionId: string) => {
    if (!(await getAuction(auctionId))) return;
    await auctionRepository.deleteAsync(auctionId);
  };

  const changeState = async (auctionId: string, state: State) => {
    if (!(await getAuction(auctionId))) return;
    await auctionRepository.changeStateAsync(auctionId, state);
  };

  const setAuctionId = (auctionId: string) => {
    saveAuctionId(auctionId);
  };

  return (
    <AuctionContext.Provider
      value={{
        auctions,
        auction,
        author,
        isAuthor,
        curAuctionId,
        setAuctionId,
        createAuction,
        getAuction,
        deleteAuction,
        changeState,
      }}
    >
      {children}
    </AuctionContext.Provider>
  );
};
