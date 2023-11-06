import { ChangeEvent, useContext, useState } from "react";

import "./LotPageForm.css";

import { enqueueSnackbar } from "notistack";

import { AuctionContext } from "../../../../contexts/AuctionContext";
import { LotContext } from "../../../../contexts/LotContext";
import { Auction } from "../../../../objects/Entities";

export default function LotPageForm() {
  const { curAuctionId, isAuthor } = useContext(AuctionContext);
  const { createLot } = useContext(LotContext);

  const [title, setTitle] = useState<string>("");
  const [description, setDescription] = useState<string>("");
  const [startPrice, setStartPrice] = useState<number>();
  const [betStep, setBetStep] = useState<number>();
  const [selectedImages, setSelectedImages] = useState<FileList | undefined>(
    undefined
  );

  const handleImageChange = async (event: ChangeEvent<HTMLInputElement>) => {
    const files = event.target.files;
    if (!files) return;

    if (files.length > 5) {
      enqueueSnackbar("Изображений может быть не более 5", {
        variant: "warning",
      });
      return;
    }

    setSelectedImages(files);
  };

  const resetState = () => {
    setTitle("");
    setDescription("");
    setStartPrice(0);
    setBetStep(0);
    setSelectedImages(undefined);
  };

  const createNewLot = () => {
    if (!validateCreateLot()) return;

    const formData = new FormData();

    for (const image of selectedImages!) {
      formData.append("images", image);
    }

    formData.append("name", title);
    formData.append("description", description);
    formData.append("auctionId", curAuctionId);
    formData.append("startPrice", startPrice?.toString()!);
    formData.append("betStep", betStep?.toString()!);

    createLot(formData);
    resetState();
  };

  const validateCreateLot = (): boolean => {
    if (!title || !description || !startPrice || !betStep || !selectedImages) {
      enqueueSnackbar("Заполните все данные", {
        variant: "error",
      });
      return false;
    }
    return true;
  };

  return (
    <div>
      {isAuthor && (
        <div className="input_box">
          <div className="title_create">Создайте лот</div>
          <input
            className="create_name"
            type="text"
            maxLength={30}
            value={title}
            onChange={(event) => setTitle(event.target.value)}
            placeholder="Введите название лота (до 30 символов)"
          />
          <textarea
            className="create_description"
            rows={10}
            maxLength={300}
            value={description}
            onChange={(event) => setDescription(event.target.value)}
            placeholder="Введите описание лота (до 300 символов)"
          ></textarea>
          <div className="box_inner">
            <input
              className="inner_item"
              type="text"
              value={startPrice}
              onChange={(event) => {
                if (isNaN(+event.target.value)) return;
                if (+event.target.value.startsWith("0")) return;
                if (+event.target.value.length > 9) return;

                setStartPrice(+event.target.value);
              }}
              placeholder="Введите стартовую цену"
            />
            <input
              className="inner_item"
              type="number"
              value={betStep}
              onChange={(event) => {
                if (isNaN(+event.target.value)) return;
                if (+event.target.value.startsWith("0")) return;
                if (+event.target.value.length > 9) return;

                setBetStep(+event.target.value);
              }}
              placeholder="Введите шаг ставки лота"
            />
          </div>
          <div className="photo_box">
            <div className="input__wrapper">
              <input
                name="file"
                type="file"
                id="input__file"
                className="input input__file"
                multiple
                accept="image/jpeg"
                onChange={handleImageChange}
              />
              <label htmlFor="input__file" className="input__file-button">
                <span className="input__file-icon-wrapper">
                  <img
                    className="input__file-icon"
                    alt="Выбрать файл"
                    width="25"
                  />
                </span>
                <span className="input__file-button-text">
                  Выберите изображения
                </span>
              </label>
            </div>
            <div>
              {selectedImages && (
                <div className="image_container">
                  <p className="about_image">Выбранные изображения</p>
                  <ul className="image_wrapper">
                    {Array.from(selectedImages).map((image, index) => (
                      <img
                        className="image"
                        key={index}
                        src={URL.createObjectURL(image)}
                        alt="Выбранные изображения"
                      />
                    ))}
                  </ul>
                </div>
              )}
            </div>
          </div>
          <button className="submit_create" onClick={createNewLot}>
            Создать
          </button>
        </div>
      )}
    </div>
  );
}
