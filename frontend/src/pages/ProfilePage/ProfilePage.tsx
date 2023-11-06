import "./ProfilePage.css";
import LotCard from "../../components/cards/lotCard/LotCard";
import AuctionCard from "../../components/cards/auctionCard/AuctionCard";
import { Auction, User } from "../../objects/Entities";
import { UserAuthorizationContext } from "../../contexts/UserAuthorizationContext";
import { useContext } from "react";
import { LotContext } from "../../contexts/LotContext";
import { AuctionContext } from "../../contexts/AuctionContext";

export default function ProfilePage() {
  const { user, signout } = useContext(UserAuthorizationContext);

  const { lots } = useContext(LotContext);
  const { auctions } = useContext(AuctionContext);
  const { members } = useContext(UserAuthorizationContext);

  const filteredAuctions = auctions?.filter(
    (auction) => auction.authorId === user?.id
  );

  return (
    <div className="profile_container">
      <div className="main_info">
        <img className="profile_image" alt="Изображение профиля" />
        <div className="profile_data">
          <div className="user_name">{user?.name}</div>
          <div className="user_email">{user?.email}</div>
        </div>
        <div className="button_box">
          <button className="signout_button" onClick={signout}>
            Выйти
          </button>
        </div>
      </div>
      <div className="activity_box">
        <div className="box_title">Ваши аукционы</div>
        <div className="box_items_auction">
          {!filteredAuctions?.length ? (
            <div className="main_empty">
              <div className="empty">
                <div>Вы пока не организовывали аукционы</div>
                <div>Пора начинать!</div>
              </div>
            </div>
          ) : (
            filteredAuctions
              ?.map((auction: Auction) => (
                <AuctionCard
                  key={auction.id}
                  auction={auction}
                  author={
                    members?.find(
                      (member: User) => member.id === auction.authorId
                    )!
                  }
                />
              ))
              .reverse()
          )}
        </div>
      </div>
      <div className="activity">
        <div className="activity_box">
          <div className="box_title">Ваше участие</div>
          <div className="box_items">
            {!lots?.length ? (
              <div className="main_empty">
                <div className="empty">
                  <div>Вы еще не делали ставок!</div>
                </div>
              </div>
            ) : (
              lots
                .map((lot, index) => (
                  <LotCard key={lot.id} lot={lot} number={index} />
                ))
                .reverse()
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
