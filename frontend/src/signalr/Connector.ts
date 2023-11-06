import * as signalR from "@microsoft/signalr";

class Connector {
  private connection: signalR.HubConnection;
  private URL = process.env.HUB_ADDRESS ?? "https://localhost:7132/auction";

  static instance: Connector;

  constructor() {
    this.connection = new signalR.HubConnectionBuilder()
      .withUrl(this.URL)
      .withAutomaticReconnect()
      .build();

    this.connection.start();
  }

  public static getInstance(): Connector {
    if (!Connector.instance) Connector.instance = new Connector();
    return Connector.instance;
  }
}

export default Connector.getInstance;
