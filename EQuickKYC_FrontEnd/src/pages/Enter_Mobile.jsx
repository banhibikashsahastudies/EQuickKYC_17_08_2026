import { useState } from 'react'
import { useNavigate } from 'react-router-dom'

function Index() {
  const [phone, setPhone] = useState('')
  const navigate = useNavigate()

  const handleContinue = () => {
     
    if (!/^\d{10}$/.test(phone)) {
      alert('Please enter a valid 10-digit mobile number')
      return
    }

    navigate('/verify_mobile_otp')
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

export default Index