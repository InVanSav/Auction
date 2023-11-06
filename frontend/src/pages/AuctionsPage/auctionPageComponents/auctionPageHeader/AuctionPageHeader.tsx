import { enqueueSnackbar } from "notistack";
import { useContext, useState } from "react";

import "./AuctionPageHeader.css";
import { UserAuthorizationContext } from "../../../../contexts/UserAuthorizationContext";
import { AuctionContext } from "../../../../contexts/AuctionContext";

export default function AuctionPageHeader() {
  const [title, setTitle] = useState<string>("");
  const [description, setDescription] = useState<string>("");

  const { createAuction: createAuctionContext } = useContext(AuctionContext);
  const { user } = useContext(UserAuthorizationContext);

  const resetState = () => {
    setTitle("");
    setDescription("");
  };

  const validateCreation = (): boolean => {
    if (!title || !description) {
      enqueueSnackbar("Заполните все поля", {
        variant: "error",
      });
      return false;
    }

    return true;
  };

  const createAuction = async () => {
    if (!validateCreation()) return;

    await createAuctionContext(title, description, user?.id!);

    resetState();
  };

  return (
    <div className="input_box">
      <div className="title_create">Создайте свой аукцион</div>
      <input
        className="create_name"
        type="text"
        value={title}
        maxLength={30}
        onChange={(event) => setTitle(event.target.value)}
        placeholder="Введите название аукциона (до 30 символов)"
      />
      <textarea
        className="create_description"
        rows={10}
        value={description}
        maxLength={300}
        onChange={(event) => setDescription(event.target.value)}
        placeholder="Введите описание аукциона (до 300 символов)"
      ></textarea>
      <button className="submit_create" onClick={createAuction}>
        Создать
      </button>
    </div>
  );
}
