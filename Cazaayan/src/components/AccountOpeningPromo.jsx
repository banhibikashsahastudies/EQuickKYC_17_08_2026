import AndroidQR from "../assets/images/android-qr.png";
import IphoneQR from "../assets/images/iphone-qr.png";

function AccountOpeningPromo() {
  return (
    <div className="lft-content-box">
      <h1>Open Your Trading Account in 5 Minutes</h1>
      <ul>
        <li>Transform your account opening with Real-Time AI Analytics.</li>
        <li>Our sophisticated AI Algorithms will enhance the process of your onboarding.</li>
        <li>Keep your PAN Card, Aadhaar Card, Signature Image, Bank Proof, Income Proof (Optional), and Aadhaar linked mobile number handy.</li>
      </ul>
      <div className="footer-qr-prt mt-5">
          <div className="qr-bx">
              <img src={AndroidQR} alt="Android QR Code" />
              <p>
                  <span>Download </span>android app
              </p>
          </div>
          <div className="qr-bx">
              <img src={IphoneQR} alt="iPhone QR Code" />
              <p>
                  <span>Download </span>iPhone app
              </p>
          </div>
      </div>
    </div>
  );
}

export default AccountOpeningPromo;
