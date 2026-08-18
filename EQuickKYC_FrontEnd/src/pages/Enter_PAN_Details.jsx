import { useKyc } from '../context/KycContext'
import API_BASE_URL from '../components/base_Url'

function Enter_PAN_Details() {
    const {
        pan,
        setPan,
        name,
        setName,
        dob,
        setDob
    } = useKyc()

    const verify_pan = async (pan, dob, name) => {
        const response = await fetch(`${API_BASE_URL}/User/pan`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                pan: pan,
                name: name,
                dob: dob
            })
        })

        return response
    }

    const submit_pan = async () => {
        const panPattern = /^[A-Z]{5}[0-9]{4}[A-Z]$/

        if (!panPattern.test(pan)) {
            alert('Please enter a valid PAN number')
            return
        }

        if (!name.trim()) {
            alert('Please enter your name')
            return
        }

        if (!dob) {
            alert('Please enter your date of birth')
            return
        }

        const birthDate = new Date(dob)
        const today = new Date()

        let age = today.getFullYear() - birthDate.getFullYear()
        const monthDifference = today.getMonth() - birthDate.getMonth()

        if (
            monthDifference < 0 ||
            (monthDifference === 0 && today.getDate() < birthDate.getDate())
        ) {
            age--
        }

        if (age < 18) {
            alert('You must be at least 18 years old')
            return
        }

        console.log('PAN:', pan)
        console.log('Name:', name)
        console.log('DOB:', dob)
        console.log('Age:', age)

        // Demo verification
        alert('PAN details are valid')

        // Backend verification later
        // try {
        //     const response = await verify_pan(pan, dob, name)
        //
        //     if (response.status === 200) {
        //         alert('PAN details are valid')
        //     } else {
        //         alert('PAN details are invalid')
        //     }
        // } catch (error) {
        //     console.error('PAN verification error:', error)
        //     alert('Unable to verify PAN. Please try again.')
        // }
    }

    return (
        <div className="min-h-screen bg-gray-100 flex items-center justify-center px-4">
            <div className="w-full max-w-md bg-white rounded-2xl shadow-lg p-8">

                <h1 className="text-3xl font-bold text-gray-900 text-center">
                    Enter PAN Details
                </h1>

                <p className="text-gray-500 text-center mt-2">
                    Enter your PAN details to continue
                </p>

                <div className="mt-8 space-y-5">

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                            PAN
                        </label>

                        <input
                            type="text"
                            value={pan}
                            onChange={(e) => setPan(e.target.value.toUpperCase())}
                            placeholder="ABCDE1234F"
                            maxLength="10"
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg uppercase focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                            Name
                        </label>

                        <input
                            type="text"
                            value={name}
                            onChange={(e) => setName(e.target.value)}
                            placeholder="Enter your name"
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        />
                    </div>

                    <div>
                        <label className="block text-sm font-medium text-gray-700 mb-2">
                            Date of Birth
                        </label>

                        <input
                            type="date"
                            value={dob}
                            onChange={(e) => setDob(e.target.value)}
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        />
                    </div>

                    <button
                        type="button"
                        onClick={submit_pan}
                        className="w-full mt-2 bg-blue-600 text-white py-3 rounded-lg font-medium hover:bg-blue-700 transition"
                    >
                        Verify PAN
                    </button>

                </div>
            </div>
        </div>
    )
}

export default Enter_PAN_Details