import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

function Enter_Email() {
  const [email, setEmail] = useState('')
  const navigate = useNavigate()

  const handleContinue = () => {
    console.log('Email:', email)

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      alert('Please enter a valid email address')
      return
    }

    navigate('/verify_email_otp')
  }

  return (
    <div className="min-h-screen bg-gray-100 flex items-center justify-center px-4">
      <div className="w-full max-w-md bg-white rounded-2xl shadow-lg p-8">
        <h1 className="text-3xl font-bold text-gray-900 text-center">
          Enter Email
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
            onChange={(e) => setEmail(e.target.value)}
            placeholder="Enter your email"
            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
          />

          <button
            type="button"
            onClick={handleContinue}
            className="w-full mt-6 bg-blue-600 text-white py-3 rounded-lg font-medium hover:bg-blue-700 transition"
          >
            Continue
          </button>
        </div>
      </div>
    </div>
  )
}

export default Enter_Email