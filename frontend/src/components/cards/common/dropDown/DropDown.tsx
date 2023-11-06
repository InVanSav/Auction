import { State } from "../../../../objects/Enums";
import "./DropDown.css";

export default function DropDown(props: { executer: (state: State) => void }) {
  return (
    <div className="menu">
      <button
        className="button_item"
        onClick={() => props.executer(State.awaiting)}
      >
        Ожидание
      </button>
      <button
        className="button_item"
        onClick={() => props.executer(State.editing)}
      >
        Редактирование
      </button>
      <button
        className="button_item"
        onClick={() => props.executer(State.running)}
      >
        Запущен
      </button>
      <button
        className="button_item"
        onClick={() => props.executer(State.canceled)}
      >
        Отменен
      </button>
    </div>
  );
}
