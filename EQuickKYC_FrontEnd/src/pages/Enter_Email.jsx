import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import API_BASE_URL from '../components/base_Url'
import { useKyc } from '../context/KycContext'

function Enter_Email() {
  const { email, setEmail } = useKyc()

  const [error, setError] = useState('')
  const [loading, setLoading] = useState(false)

  const navigate = useNavigate()

  async function sendEmail(email) {
    const response = await fetch(`${API_BASE_URL}/User/email`, {
      method: "POST",
      headers: {
        "Content-Type": "application/json"
      },
      body: JSON.stringify({ email })
    })

    return response
  }

  const handleContinue = async () => {
    setError('')

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      setError('Please enter a valid email address')
      return
    }

    // Demo navigation
    navigate('/verify_email_otp')

    // Backend implementation later
    // try {
    //   setLoading(true)
    //
    //   const response = await sendEmail(email)
    //
    //   if (response.status === 200) {
    //     navigate('/verify_email_otp')
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
          Enter your email address to continue
        </p>

        <div className="mt-8">

          <label className="block text-sm font-medium text-gray-700 mb-2">
            Email Address
          </label>

          <input
            type="email"
            value={email}
            onChange={(e) => {
              setEmail(e.target.value)
              setError('')
            }}
            placeholder="Enter email address"
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

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

export default Enter_Email