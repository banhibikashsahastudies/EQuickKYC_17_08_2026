import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

function Verify_Mobile_OTP() {
  const [otp, setOtp] = useState('')
  const navigate = useNavigate()

  const verify_otp_mobile = () => {
    console.log('Verifying mobile OTP:', otp)

    if (otp !== '111111') {
      alert('Invalid OTP')
      return
    }

    alert('Mobile OTP verified successfully')
    navigate('/enter_email')
  }

  const resend_mobile_otp = () => {
    console.log('OTP sent again')
    alert('OTP sent again')
  }

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center px-4">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-lg p-8">
        <h1 className="text-3xl font-bold text-gray-900 text-center">
          Verify Mobile OTP
        </h1>

        <p className="text-gray-500 text-center mt-2">
          Enter the OTP sent to your mobile number
        </p>

        <div className="mt-8">
          <label className="block text-sm font-medium text-gray-700 mb-2">
            OTP
          </label>

          <input
            type="text"
            value={otp}
            onChange={(e) => setOtp(e.target.value.replace(/\D/g, ''))}
            placeholder="Enter 6-digit OTP"
            maxLength="6"
            className="w-full px-4 py-3 border border-gray-300 rounded-lg text-center text-xl tracking-widest focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

          <button
            type="button"
            onClick={verify_otp_mobile}
            className="w-full mt-6 bg-blue-600 text-white py-3 rounded-lg font-medium hover:bg-blue-700 transition"
          >
            Verify OTP
          </button>

          <button
            type="button"
            onClick={resend_mobile_otp}
            className="w-full mt-3 text-blue-600 font-medium hover:text-blue-700"
          >
            Resend OTP
          </button>
        </div>
      </div>
    </div>
  )
}

export default Verify_Mobile_OTP