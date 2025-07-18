import { Routes, Route } from "react-router-dom";
import UserLoginPage from "./features/user/pages/UserLoginPage/UserLoginPage";
import TicketingPage from "./features/ticket/pages/TicketingPage/TicketingPage";

import './App.css'

function App() {
  return (
    <>
      <Routes>
      <Route path="/" element={<UserLoginPage />} />
      <Route path="/ticketing" element={<TicketingPage />} />
    </Routes>
    </>
  )
}

export default App
