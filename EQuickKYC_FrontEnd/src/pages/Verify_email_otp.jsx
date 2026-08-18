import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

function Verify_email_otp() {
  const [otp, setOtp] = useState('')
  const navigate = useNavigate()

  const verify_otp_email = () => {
    console.log('Verifying email OTP:', otp)

    if (otp !== '111111') {
      alert('Invalid OTP')
      return
    }

    alert('Email OTP verified successfully')
    navigate('/enter_pan_details')
  }

  const resend_email_otp = () => {
    console.log('Email OTP sent again')
    alert('Email OTP sent again')
  }

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center px-4">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-lg p-8">
        <h1 className="text-3xl font-bold text-gray-900 text-center">
          Verify Email OTP
        </h1>

        <p className="text-gray-500 text-center mt-2">
          Enter the OTP sent to your email address
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
            onClick={verify_otp_email}
            className="w-full mt-6 bg-blue-600 text-white py-3 rounded-lg font-medium hover:bg-blue-700 transition"
          >
            Verify OTP
          </button>

          <button
            type="button"
            onClick={resend_email_otp}
            className="w-full mt-3 text-blue-600 font-medium hover:text-blue-700"
          >
            Resend OTP
          </button>
        </div>
      </div>
    </div>
  )
}

export default Verify_email_otp