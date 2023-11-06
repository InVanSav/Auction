import { useContext, useState } from "react";

import "./LotPageHeader.css";

import { enqueueSnackbar } from "notistack";

import { AuctionContext } from "../../../../contexts/AuctionContext";
import { Auction } from "../../../../objects/Entities";
import { State, getStateFromEnum } from "../../../../objects/Enums";
import DropDown from "../../../../components/cards/common/dropDown/DropDown";

export default function LotPageHeader() {
  const { author, isAuthor, auction } = useContext(AuctionContext);
  const [open, setOpen] = useState(false);

  const { deleteAuction, changeState, curAuctionId } =
    useContext(AuctionContext);

  const isChangable =
    auction?.state == State.running ||
    auction?.state == State.editing ||
    auction?.state == State.awaiting;

  const isEndValid = auction?.dateStart! < auction?.dateEnd!;

  const handleOpen = () => {
    setOpen(!open);
  };

  const changeStateCurAuction = async (state: State) => {
    if (!curAuctionId) return;

    if (auction?.state == state) {
      enqueueSnackbar("Выберите статус отличный от текущего", {
        variant: "warning",
      });
      return;
    }

    await changeState(curAuctionId, state);
  };

  const deleteCurAuction = async () => {
    if (!curAuctionId) return;
    await deleteAuction(curAuctionId);
  };

  return (
    <div>
      <div className="about_box">
        <div className="main_information">
          <div className="about_title">{auction?.name}</div>
          <div className="about_description">{auction?.description}</div>
        </div>
        <div className="adding_info">
          <div className="wrapper">
            <div className="sub_title">Статус:</div>
            <div className="info_status">
              {getStateFromEnum(auction?.state!)}
            </div>
          </div>
          <div className="wrapper">
            <div className="sub_title">Автор:</div>
            <div className="info_owner">{author?.name}</div>
          </div>
          <div className="wrapper">
            <div className="sub_title">Начало:</div>
            <div className="date_start">
              {new Date(auction?.dateStart!).toLocaleString()}
            </div>
          </div>
          {isEndValid && (
            <div className="wrapper">
              <div className="sub_title">Конец:</div>
              <div className="date_end">
                {new Date(auction?.dateEnd!).toLocaleString()}
              </div>
            </div>
          )}
        </div>
        {isAuthor && (
          <div className="button_auction_box">
            {isChangable && (
              <div className="button_auction_box">
                <button className="button_item" onClick={handleOpen}>
                  <img
                    className="image_item change_status"
                    alt="Изменить статус"
                  />
                </button>
                {open && <DropDown executer={changeStateCurAuction} />}
                {/* <button className="button_item">
                  <img className="image_item edit" alt="Редактировать" />
                </button> */}
                <button
                  className="button_item danger_button"
                  onClick={() => changeStateCurAuction(State.completed)}
                >
                  <img className="image_item completed" alt="Завершить" />
                </button>
              </div>
            )}
          </div>
        )}
      </div>
      {isAuthor && (
        <div className="help_div">
          <button
            className="button_item danger_button danger_delete_button"
            onClick={deleteCurAuction}
          >
            <img className="image_item delete" alt="Удалить" />
          </button>
        </div>
      )}
    </div>
  );
}
