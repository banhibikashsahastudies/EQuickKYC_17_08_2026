import { createContext, useContext, useState } from 'react'

const KycContext = createContext()

export function KycProvider({ children }) {
    const [phone, setPhone] = useState('')
    const [email, setEmail] = useState('')
    const [pan, setPan] = useState('')
    const [name, setName] = useState('')
    const [dob, setDob] = useState('')

    return (
        <KycContext.Provider
            value={{
                phone,
                setPhone,
                email,
                setEmail,
                pan,
                setPan,
                name,
                setName,
                dob,
                setDob
            }}
        >
            {children}
        </KycContext.Provider>
    )
}

export function useKyc() {
    return useContext(KycContext)
}