import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../assets/css/banhiStyle.css';

function AccountType() {
    const [Account, setAccount] = useState('');
    const navigate = useNavigate();

    const handle_account_choosing = (type) => {
        setAccount(type);

        //setting session data
        sessionStorage.setItem('AccountType', type)

        console.log('Selected Account:', sessionStorage.getItem('AccountType'));

        //navigation to Mobile page on selection and set the last page as Account Type
        sessionStorage.setItem('Last Page', 'Account Type')
        navigate("/")
    };

    const handleBack = () => {
        sessionStorage.removeItem('Phone')
        navigate('/')
    }

    return (
        <div className="demo">
            <button
                type="button"
                className="back-link"
                onClick={handleBack}
            >
                <i
                    className="fa-solid fa-arrow-left"
                    aria-hidden="true"
                ></i>
                <span>Back</span>
            </button>
            <h1>Choose Your Account Type</h1>

            <div className="account-container">

                <div className="account-card">
                    <h2>Individual</h2>
                    <p>For individual account holders</p>

                    <ul>
                        <li>✓ Personal account</li>
                        <li>✓ PAN + Aadhaar required</li>
                        <li>✓ Quick account opening</li>
                        <li>✓ Easy KYC verification</li>
                    </ul>

                    <button onClick={() => handle_account_choosing('Individual')}>
                        Create Individual Account
                    </button>
                </div>

                <div className="account-card">
                    <h2>HUF</h2>
                    <p>For Hindu Undivided Family accounts</p>

                    <ul>
                        <li>✓ HUF account</li>
                        <li>✓ Karta details required</li>
                        <li>✓ PAN + KYC verification</li>
                        <li>✓ Family account support</li>
                    </ul>

                    <button onClick={() => handle_account_choosing('HUF')}>
                        Create HUF Account
                    </button>
                </div>

                <div className="account-card">
                    <h2>Minor</h2>
                    <p>For accounts opened for minors</p>

                    <ul>
                        <li>✓ Minor account</li>
                        <li>✓ Guardian details required</li>
                        <li>✓ PAN + KYC verification</li>
                        <li>✓ Secure account opening</li>
                    </ul>

                    <button onClick={() => handle_account_choosing('Minor')}>
                        Create Minor Account
                    </button>
                </div>

            </div>
        </div>
    );
}

export default AccountType;