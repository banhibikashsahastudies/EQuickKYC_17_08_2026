import React from "react";
import { Container } from "react-bootstrap";

function Footer() {
  return (
    <>
      <footer className="footer">
        <Container className="d-flex align-items-center justify-content-between">
          <p className="mb-0">© 2026 <a href="https://www.cazaayan.com/" target="_blank" rel="noopener noreferrer">Cazaayan.</a> All Rights Reserved. SEBI Registered Broker.</p>
          
          <ul className="footer-links mb-0">
            <li><a href="#">Privacy Policy</a></li>
            <li><a href="#">Terms & Conditions</a></li>
            <li><a href="#">Risk Disclosure</a></li>
            <li><a href="#">Contact Us</a></li>
          </ul>
        </Container>
      </footer>
    </>
  );
}

export default Footer;