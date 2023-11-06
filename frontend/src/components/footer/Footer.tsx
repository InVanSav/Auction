import "./Footer.css";

export default function Footer() {
  return (
    <footer className="footer">
      <div className="footer_container">
        <div className="container_sources">
          <a
            className="source_github"
            href="https://github.com/InVanSav/Auction"
            rel="noreferrer"
            target="_blank"
          >
            <img className="github" alt="Исходники" />
          </a>
          <div className="source_text">
            <div>Savickij Ivan.</div>
            <div>Source code.</div>
          </div>
        </div>
      </div>
    </footer>
  );
}
