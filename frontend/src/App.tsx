import { BrowserRouter as Router, Routes, Route } from "react-router-dom";

import Header from "./components/header/Header";
import Footer from "./components/footer/Footer";
import Arrow from "./components/arrow/Arrow";

import AuctionsPage from "./pages/AuctionsPage/AuctionsPage";
import AuthorizationPage from "./pages/AuthorizationPage/AuthorizationPage";
import LotsPage from "./pages/LotsPage/LotsPage";

import "./App.css";
import { AuctionProvider } from "./contexts/AuctionContext";
import { LotProvider } from "./contexts/LotContext";
import Breadcrumbs from "./components/breadcrumbs/Breadcrumbs";
import { UserAuthorizationProvider } from "./contexts/UserAuthorizationContext";

import NotFoundPage from "./pages/NotFoundPage/NotFoundPage";
import ProfilePage from "./pages/ProfilePage/ProfilePage";

function App() {
  return (
    <div className="App">
      <Router>
        <UserAuthorizationProvider>
          <Header />
          <Breadcrumbs separator=">" />
          <AuctionProvider>
            <LotProvider>
              <Routes>
                <Route index element={<AuctionsPage />}></Route>
                <Route
                  path="/authorization"
                  element={<AuthorizationPage />}
                ></Route>
                <Route path="/lots" element={<LotsPage />}></Route>
                <Route path="/profile" element={<ProfilePage />}></Route>
                <Route path="*" element={<NotFoundPage />} />
              </Routes>
            </LotProvider>
          </AuctionProvider>
        </UserAuthorizationProvider>
      </Router>
      <Arrow />
      <Footer />
    </div>
  );
}

export default App;
