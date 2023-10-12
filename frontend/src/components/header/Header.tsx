import { useEffect, useState } from "react";
import { Link } from "react-router-dom";

import "./Header.css";

export default function Header() {
  const [isHeaderFixed, setIsHeaderFixed] = useState(false);

  useEffect(() => {
    function handleScroll() {
      const scrollY = window.scrollY;
      const header = document.querySelector(".header");
      const headerHeight = header?.clientHeight || 0;

      if (scrollY >= headerHeight) {
        setIsHeaderFixed(true);
      } else {
        setIsHeaderFixed(false);
      }
    }

    window.addEventListener("scroll", handleScroll);

    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  return (
    <header className={`header ${isHeaderFixed ? "fixed" : ""}`}>
      <div className="header_container">
        <div className="container_logo">
          <img className="logo" alt="Логотип" />
          <div className="logo_text">Auctions</div>
        </div>
        <div className="container_tools">
          <Link to="/profile">
            <button className="tool_item">
              <img className="item_img user" alt="Профиль" />
            </button>
          </Link>
          <Link to="/search">
            <button className="tool_item">
              <img className="item_img search" alt="Поиск" />
            </button>
          </Link>
        </div>
      </div>
    </header>
  );
}
