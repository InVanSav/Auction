import { useEffect, useState, useContext } from "react";

import { Lot } from "../../objects/Entities";

import { LotContext } from "../../contexts/LotContext";

import LotPageContent from "./lotPageComponents/lotPageContent/LotPageContent";
import LotPageForm from "./lotPageComponents/lotPageForm/LotPageForm";
import LotPageHeader from "./lotPageComponents/lotPageHeader/LotPageHeader";

import Connector from "../../signalr/Connector";
import { getStateFromEnum } from "../../objects/Enums";

import { enqueueSnackbar } from "notistack";
import { AuctionContext } from "../../contexts/AuctionContext";
import { useNavigate } from "react-router-dom";

export default function LotsPage() {
  const [lots, setLots] = useState<Lot[] | undefined>([]);
  const { getLotsByAuction } = useContext(LotContext);

  const { curAuctionId } = useContext(AuctionContext);
  const navigate = useNavigate();

  const { soldLot, changedLotStatus, createdLot, madeBet } = Connector();

  useEffect(() => {
    if (!curAuctionId) {
      navigate("/");
      return;
    }

    const getLots = async () => {
      setLots(await getLotsByAuction());
    };

    getLots();
  }, []);

  useEffect(() => {
    madeBet((lotName, username) => {
      enqueueSnackbar(
        <div>
          <div>Пользователь {username} сделал ставку</div>
          <div>по лоту {lotName}</div>
        </div>,
        {
          variant: "info",
        }
      );
    });
  }, [madeBet]);

  useEffect(() => {
    createdLot((auctionName, lotName) => {
      enqueueSnackbar(
        <div>
          <div>В аукцион {auctionName}</div>
          <div>добавлен лот {lotName}</div>
        </div>,
        {
          variant: "info",
        }
      );
    });
  }, [createdLot]);

  useEffect(() => {
    changedLotStatus((auctionName, lotName, state) => {
      const textState = getStateFromEnum(state);
      enqueueSnackbar(
        <div>
          <div>Смена статуса: {lotName}</div>
          <div>на {textState}</div>
          <div>в аукционе {auctionName}</div>
        </div>,
        {
          variant: "info",
        }
      );
    });
  }, [changedLotStatus]);

  useEffect(() => {
    soldLot((auctionName, lotName, buyoutPrice) => {
      enqueueSnackbar(
        <div>
          <div>Выкуплен лот {lotName}</div>
          <div>за {buyoutPrice}</div>
          <div>в аукционе {auctionName}</div>
        </div>,
        {
          variant: "info",
        }
      );
    });
  }, [soldLot]);

  return (
    <div className="main_box">
      <LotPageHeader />
      <LotPageForm />
      <LotPageContent lots={lots!} />
    </div>
  );
}
