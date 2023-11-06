import LotCard from "../../../../components/cards/lotCard/LotCard";
import { Lot } from "../../../../objects/Entities";

import "./LotPageContent.css";

export default function LotPageContent(props: { lots: Lot[] }) {
  return (
    <div className="main_container_lots">
      {!props.lots?.length ? (
        <div className="main_empty">
          <div className="empty">
            <div>Лотов пока нет.</div>
          </div>
        </div>
      ) : (
        props.lots.map((lot, index) => <LotCard key={lot.id} lot={lot} number={index}/>).reverse()
      )}
    </div>
  );
}
