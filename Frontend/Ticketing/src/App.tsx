import { Routes, Route } from "react-router-dom";
import HomePage from "./HomePage";
import UserPage from "./UserPage";

import './App.css'

function App() {
  return (
    <>
      <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/user/:username" element={<UserPage />} />
    </Routes>
    </>
  )
}

export default App
