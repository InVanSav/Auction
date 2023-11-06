import ReactDOM from "react-dom/client";

import App from "./App";

import "./index.css";
import { SnackbarProvider, closeSnackbar } from "notistack";

const root = ReactDOM.createRoot(
  document.getElementById("root") as HTMLElement
);

root.render(
  <SnackbarProvider
    maxSnack={3}
    action={(snackbarId) => (
      <button className="dismiss_btn" onClick={() => closeSnackbar(snackbarId)}>
        <img className="dismiss_image" alt="Dismiss" />
      </button>
    )}
    anchorOrigin={{ horizontal: "right", vertical: "top" }}
  >
    <App />
  </SnackbarProvider>
);
