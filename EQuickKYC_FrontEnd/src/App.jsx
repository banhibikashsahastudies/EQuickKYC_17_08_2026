import { BrowserRouter, Routes, Route } from 'react-router-dom'
import Enter_Mobile from './pages/Enter_Mobile'
import Verify_Mobile_OTP from './pages/Verify_mobile_OTP'
import Enter_Email from './pages/Enter_Email'
import Verify_email_otp from './pages/Verify_email_otp'
import Enter_PAN_Details from './pages/Enter_PAN_Details'

function App() {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/" element={<Enter_Mobile />} />
        <Route path="/verify_mobile_otp" element={<Verify_Mobile_OTP />} />
        <Route path="/enter_email" element={<Enter_Email />} />
        <Route path="/verify_email_otp" element={<Verify_email_otp />} />
        <Route path="/enter_pan_details" element={<Enter_PAN_Details />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App