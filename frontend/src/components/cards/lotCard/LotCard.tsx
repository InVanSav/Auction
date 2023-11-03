import { Lot } from "../../../objects/Entities";

import "./LotCard.css";
import { State, getStateFromEnum } from "../../../objects/Enums";
import { useContext, useState } from "react";
import { AuctionContext } from "../../../contexts/AuctionContext";
import { enqueueSnackbar } from "notistack";
import { LotContext } from "../../../contexts/LotContext";
import DropDown from "../common/dropDown/DropDown";

export default function LotCard(props: { lot: Lot }) {
  const [open, setOpen] = useState(false);

  const { curAuctionId, isAuthor, auction } = useContext(AuctionContext);
  const { changeState, deleteLot, doBet } = useContext(LotContext);

  const isChangable =
    props.lot?.state == State.running ||
    props.lot?.state == State.editing ||
    props.lot?.state == State.awaiting;

  const canDoBet = props.lot?.state == State.running;

  const buyoutLot =
    props.lot.buyoutPrice === 0 ? "Не выкуплен" : `${props.lot.buyoutPrice}р.`;

  const curBet = props.lot.bets
    ? Math.max(...props.lot.bets.map((bet) => bet.value))
    : "Ставок нет";

  const handleOpen = () => {
    setOpen(!open);
  };

  const changeStateCurLot = async (state: State) => {
    if (!curAuctionId) return;

    if (auction?.state !== State.running) {
      enqueueSnackbar("Аукцион не запущен, запустите аукцион", {
        variant: "warning",
      });
      return;
    }

    if (props.lot?.state == state) {
      enqueueSnackbar("Выберите статус отличный от текущего", {
        variant: "warning",
      });
      return;
    }

    await changeState(props.lot.id, state);
  };

  const deleteCurLot = () => {
    if (canDoBet) {
      enqueueSnackbar("Лот учавствует в торгах", {
        variant: "warning",
      });
      return;
    }

    deleteLot(props.lot.id);
  };

  const doNewBet = () => {
    if (!canDoBet) {
      enqueueSnackbar("Лот не учавствует в торгах", {
        variant: "warning",
      });
      return;
    }

    doBet(props.lot.id);
  };

  return (
    <div className="card_container">
      <div className="title">{props.lot.name}</div>
      <div className="state">Статус: {getStateFromEnum(props.lot.state)}</div>
      <div className="description">{props.lot.description}</div>
      <div className="image_box">
        {props.lot.images.map((image, index) => (
          <div key={index}>
            <img
              className="image"
              src={`data:image/jpeg;base64, ${image.data}`}
              alt={image.name}
            />
          </div>
        ))}
      </div>
      <div className="container_info_lot">
        <div className="info_lot_item main_item">
          <div className="info_lot_item child_item">
            <div className="info_lot_subitem">
              <div className="info_price">Начальная цена:</div>
              <div className="price_text">{props.lot.startPrice}p.</div>
            </div>
            <div className="info_lot_subitem">
              <div className="info_price">Шаг ставки:</div>
              <div className="price_text">{props.lot.betStep}p.</div>
            </div>
          </div>
          <div className="info_lot_item child_item">
            <div className="info_lot_subitem">
              <div className="info_price current_value">Текущая ставка:</div>
              <div className="price_text">{curBet}p.</div>
            </div>
            <div className="info_lot_subitem">
              <div className="info_price current_value">Цена выкупа:</div>
              <div className="price_text">{buyoutLot}</div>
            </div>
          </div>
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
                {open && <DropDown executer={changeStateCurLot} />}
                {/* <button className="button_item">
                  <img className="image_item edit" alt="Редактировать" />
                </button> */}
                <button
                  className="button_item danger_button"
                  onClick={() => changeStateCurLot(State.completed)}
                >
                  <img className="image_item completed" alt="Завершить" />
                </button>
              </div>
            )}
          </div>
        )}
      </div>
      {canDoBet && (
        <button className="do_bet_button" onClick={doNewBet}>
          Сделать ставку
        </button>
      )}
      {isAuthor && (
        <div className="help_div">
          <button
            className="button_item danger_button danger_delete_button"
            onClick={deleteCurLot}
          >
            <img className="image_item delete" alt="Удалить" />
          </button>
        </div>
      )}
    </div>
  );
}
