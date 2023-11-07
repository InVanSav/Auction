import * as signalR from "@microsoft/signalr";
import { State } from "../objects/Enums";

class Connector {
  private connection: signalR.HubConnection;
  private URL = "http://localhost:7132/notices";

  static instance: Connector;

  public madeBet: (
    onMadeBet: (lotName: string, username: string) => void
  ) => void;

  public createdLot: (
    onCreatedLot: (auctionName: string, lotName: string) => void
  ) => void;

  public changedAuctionStatus: (
    onChangedAuctionStatus: (auctionName: string, state: State) => void
  ) => void;

  public changedLotStatus: (
    onChangedLotStatus: (
      auctionName: string,
      lotName: string,
      state: State
    ) => void
  ) => void;

  public soldLot: (
    onSoldLot: (
      auctionName: string,
      lotName: string,
      buyoutPrice: number
    ) => void
  ) => void;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.URL)
      .withAutomaticReconnect()
      .build();

    this.connection.start();

    this.madeBet = (onMadeBet) => {
      this.connection.on("MadeBet", (lotName, username) => {
        onMadeBet(lotName, username);
      });
    };

    this.createdLot = (onCreatedLot) => {
      this.connection.on("CreatedLot", (auctionName, lotName) => {
        onCreatedLot(auctionName, lotName);
      });
    };

    this.changedAuctionStatus = (onChangedAuctionStatus) => {
      this.connection.on("ChangedAuctionStatus", (auctionName, state) => {
        onChangedAuctionStatus(auctionName, state);
      });
    };

    this.changedLotStatus = (onChangedLotStatus) => {
      this.connection.on("ChangedLotStatus", (auctionName, lotName, state) => {
        onChangedLotStatus(auctionName, lotName, state);
      });
    };

    this.soldLot = (onSoldLot) => {
      this.connection.on("SoldLot", (auctionName, lotName, buyoutPrice) => {
        onSoldLot(auctionName, lotName, buyoutPrice);
      });
    };
  }

  public static getInstance(): Connector {
    if (!Connector.instance) Connector.instance = new Connector();
    return Connector.instance;
  }
}

export default Connector.getInstance;
