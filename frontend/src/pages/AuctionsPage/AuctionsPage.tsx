import { useEffect } from "react";

import { getStateFromEnum } from "../../objects/Enums";
import Connector from "../../signalr/Connector";

import AuctionPageContent from "./auctionPageComponents/auctionPageContent/AuctionPageContent";
import AuctionPageHeader from "./auctionPageComponents/auctionPageHeader/AuctionPageHeader";

import { enqueueSnackbar } from "notistack";

export default function AuctionsPage() {
  const { changedAuctionStatus } = Connector();

  useEffect(() => {
    changedAuctionStatus((auctionName, state) => {
      const textState = getStateFromEnum(state);
      enqueueSnackbar(
        <div>
          <div>Смена статуса: {textState}</div>
          <div>в аукционе {auctionName}</div>
        </div>,
        {
          variant: "info",
        }
      );
    });
  }, [changedAuctionStatus]);

  return (
    <div className="main_box">
      <AuctionPageHeader />
      <AuctionPageContent />
    </div>
  );
}
