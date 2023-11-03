import { useEffect, useState, useContext } from "react";

import { Lot } from "../../objects/Entities";

import { LotContext } from "../../contexts/LotContext";

import LotPageContent from "./lotPageComponents/lotPageContent/LotPageContent";
import LotPageForm from "./lotPageComponents/lotPageForm/LotPageForm";
import LotPageHeader from "./lotPageComponents/lotPageHeader/LotPageHeader";

export default function LotsPage() {
  const [lots, setLots] = useState<Lot[] | undefined>([]);
  const { getLotsByAuction } = useContext(LotContext);

  useEffect(() => {
    const getLots = async () => {
      setLots(await getLotsByAuction());
    };

    getLots();
  }, []);

  return (
    <div className="main_box">
      <LotPageHeader />
      <LotPageForm />
      <LotPageContent lots={lots!} />
    </div>
  );
}
