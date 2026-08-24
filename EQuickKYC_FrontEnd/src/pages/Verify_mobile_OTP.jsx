import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import API_BASE_URL from '../components/base_Url'
import { useKyc } from '../context/KycContext'

function Verify_Mobile_OTP() {
  const { phone } = useKyc()

  const [otp, setOtp] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const navigate = useNavigate()

  async function verifyMobileOtp(phone, otp) {
    const response = await fetch(`${API_BASE_URL}/User/verify-mobile-otp`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({
        phone: phone,
        otp: otp
      })
    })

    return response
  }

  const verify_otp_mobile = async () => {
    setError('')

    if (!/^\d{6}$/.test(otp)) {
      setError('Please enter a valid 6-digit OTP')
      return
    }

    // Demo OTP verification
    if (otp !== '111111') {
      setError('Invalid OTP. Please try again.')
      return
    }

    // Demo navigation
    navigate('/enter_email')

    // Backend implementation later
    // const response = await verifyMobileOtp(phone, otp)
    //
    // if (response.status === 200) {
    //   navigate('/enter_email')
    // }
  }

  const resend_mobile_otp = () => {
    setError('')
    console.log('OTP sent again')
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
            onChange={(e) => {
              setOtp(e.target.value.replace(/\D/g, ''))
              setError('')
            }}
            placeholder="Enter 6-digit OTP"
            maxLength="6"
            className="w-full px-4 py-3 border border-gray-300 rounded-lg text-center text-xl tracking-widest focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

          {error && (
            <p className="mt-3 text-sm text-red-600 text-center">
              {error}
            </p>
          )}

          <button
            type="button"
            onClick={verify_otp_mobile}
            disabled={loading}
            className="w-full mt-6 bg-blue-600 text-white py-3 rounded-lg font-medium hover:bg-blue-700 transition disabled:bg-blue-400"
          >
            {loading ? 'Verifying...' : 'Verify OTP'}
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