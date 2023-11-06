import { State } from "./Enums";

export type User = {
  id: string;
  name: string;
  email: string;
  password: string;
};

export type Bet = {
  id: string;
  value: number;
  lotId: string;
  userId: string;
  dateTime: Date;
};

export type Image = {
  name: string;
  data: string;
};

export type Lot = {
  id: string;
  name: string;
  description: string;
  auctionId: string;
  startPrice: number;
  buyoutPrice: number;
  betStep: number;
  state: State;
  bets: Bet[];
  images: Image[];
};

export type Auction = {
  id: string;
  name: string;
  description: string;
  dateStart: Date;
  dateEnd: Date;
  authorId: string;
  state: State;
  lots: Lot[];
};
