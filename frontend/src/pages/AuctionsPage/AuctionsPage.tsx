import AuctionPageContent from "./auctionPageComponents/auctionPageContent/AuctionPageContent";
import AuctionPageHeader from "./auctionPageComponents/auctionPageHeader/AuctionPageHeader";

export default function AuctionsPage() {
  return (
    <div className="main_box">
      <AuctionPageHeader />
      <AuctionPageContent />
    </div>
  );
}
