import { Routes, Route } from "react-router-dom";
import UserLoginPage from "./features/user/pages/UserLoginPage";
import UserPage from "./features/user/pages/UserPage";

import './App.css'

function App() {
  return (
    <>
      <Routes>
      <Route path="/" element={<UserLoginPage />} />
      <Route path="/user/:username" element={<UserPage />} />
    </Routes>
    </>
  )
}

export default App
