import { useState } from 'react'
import Header from './layout/Header'
import Footer from './layout/Footer'
import './App.css'
import './assets/css/style.css'
import 'bootstrap/dist/css/bootstrap.min.css';
import Mobilepage from './pages/Mobilepage'
import Emailpage from './pages/Emailpage'
import Panpage from './pages/Panpage'
import ViewPanDetails from './pages/ViewPanDetails'
import { Route, Routes } from 'react-router-dom';
import AccountType from './pages/AccountType'
import Admin from './pages/Admin'

function App() {
  const [count, setCount] = useState(0)

  return (
    <>
      <Header />
      
      <Routes>
        <Route path="/" element={<Mobilepage />} />
        <Route path="/email" element={<Emailpage />} />
        <Route path="/pan" element={<Panpage />} />
        <Route path="/pan_details" element={<ViewPanDetails />} />
        <Route path="/account_type" element={<AccountType />} />
        <Route path="/admin_dash" element={<Admin />} />
      </Routes>

      <Footer />
    </>
  )
}

export default App;