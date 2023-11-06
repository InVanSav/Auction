import { useContext } from "react";

import "./AuctionPageContent.css";

import { Auction, User } from "../../../../objects/Entities";
import AuctionCard from "../../../../components/cards/auctionCard/AuctionCard";

import { AuctionContext } from "../../../../contexts/AuctionContext";
import { UserAuthorizationContext } from "../../../../contexts/UserAuthorizationContext";

export default function AuctionPageContent() {
  const { members } = useContext(UserAuthorizationContext);
  const { auctions } = useContext(AuctionContext);

  return (
    <div className="main_container">
      {!auctions?.length ? (
        <div className="main_empty">
          <div className="empty">
            <div>Аукционов пока нет.</div>
            <div>Будьте первым и создайте свой!</div>
          </div>
        </div>
      ) : (
        auctions
          .map((auction: Auction) => (
            <AuctionCard
              key={auction.id}
              auction={auction}
              author={
                members?.find((member: User) => member.id === auction.authorId)!
              }
            />
          ))
          .reverse()
      )}
    </div>
  );
}
