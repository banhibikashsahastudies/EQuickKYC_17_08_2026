import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import './index.css'
import App from './App.jsx'
import { KycProvider } from './context/KycContext'

createRoot(document.getElementById('root')).render(
    <StrictMode>
        <KycProvider>
            <App />
        </KycProvider>
    </StrictMode>,
)