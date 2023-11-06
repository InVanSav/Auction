import { Link, useLocation } from "react-router-dom";

import "./Breadcrumbs.css";
import { useContext } from "react";
import { UserAuthorizationContext } from "../../contexts/UserAuthorizationContext";

export default function Breadcrumbs(props: { separator: string }) {
  const location = useLocation();
  const { user } = useContext(UserAuthorizationContext);
  if (!user) return <div></div>;

  const pathnames = location.pathname.split("/").filter((x) => x);

  return (
    <div className={`breadcrumb ${pathnames.length > 0 ? "" : "disactive"}`}>
      <div className="breadcrumb_item">
        <Link className="link_breadcrumb" to="/">
          Аукционы
        </Link>
        <div className="breadcrumb_separator">{props.separator}</div>
      </div>
      {pathnames.map((name, index) => {
        const routeTo = `/${pathnames.slice(0, index + 1).join("/")}`;
        const isLast = index === pathnames.length - 1;

        return isLast ? (
          <div
            key={name}
            className="breadcrumb_item active"
            aria-current="page"
          >
            {name}
          </div>
        ) : (
          <div key={name} className="breadcrumb_item">
            <Link className="link_breadcrumb" to={routeTo}>
              {name}
            </Link>
            <div className="breadcrumb_separator">{props.separator}</div>
          </div>
        );
      })}
    </div>
  );
}
