
import React from 'react'
import {Container} from 'react-bootstrap'
import Logo from '../assets/images/logo.png'

function Header(){
    return (
        <>

            <header className="header">
                <Container className='d-flex align-items-center justify-content-between'>
                    <a href="#" className="logo">
                        <img src={Logo} alt="Cazaayan logo" />
                    </a>

                    <p className="topcall">Need Assistance? &nbsp;&nbsp; Happy to help! <a href="tel:+1234567890">Call Us</a></p>
                </Container>
            </header>

        </>
    )
}

export default Header;