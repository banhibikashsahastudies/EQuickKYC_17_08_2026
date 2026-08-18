import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import API_BASE_URL from '../components/base_Url'

function Index() {
  const [phone, setPhone] = useState('')
  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const navigate = useNavigate()

  async function sendPhoneNumber(phone) {
    const response = await fetch(`${API_BASE_URL}/User/phone`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ phone })
    })

    return response
  }

  const handleContinue = async () => {
    setError('')

    if (!/^\d{10}$/.test(phone)) {
      setError('Please enter a valid 10-digit mobile number')
      return
    }

    //demo navigate
    navigate('/verify_mobile_otp',{
      state: { phone: phone }
    })

    // try {
    //   setLoading(true)

    //   const response = await sendPhoneNumber(phone)

    //   if (response.status === 200) {
    //     navigate('/verify_mobile_otp')
    //   } else {
    //     setError('Failed to send OTP. Please try again.')
    //   }
    // } catch (error) {
    //   setError('Failed to send OTP. Please try again.')
    // } finally {
    //   setLoading(false)
    // }
  }

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center px-4">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-lg p-8">

        <h1 className="text-3xl font-bold text-gray-900 text-center">
          EQuickKYC
        </h1>

        <p className="text-gray-500 text-center mt-2">
          Enter your mobile number to continue
        </p>

        <div className="mt-8">

          <label className="block text-sm font-medium text-gray-700 mb-2">
            Mobile Number
          </label>

          <div className="flex">
            <span className="flex items-center px-4 bg-gray-100 border border-r-0 border-gray-300 rounded-l-lg text-gray-600">
              +91
            </span>

            <input
              type="tel"
              value={phone}
              onChange={(e) => setPhone(e.target.value.replace(/\D/g, ''))}
              placeholder="Enter mobile number"
              maxLength="10"
              className="w-full px-4 py-3 border border-gray-300 rounded-r-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
            />
          </div>

          {error && (
            <p className="mt-3 text-sm text-red-600">
              {error}
            </p>
          )}

          <button
            type="button"
            onClick={handleContinue}
            disabled={loading}
            className="w-full mt-6 bg-blue-600 text-white py-3 rounded-lg font-medium hover:bg-blue-700 transition disabled:bg-blue-400"
          >
            {loading ? 'Sending OTP...' : 'Continue'}
          </button>

        </div>
      </div>
    </div>
  )
}

export default Index