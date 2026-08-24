import { useEffect, useRef, useState } from "react";
import { Container, Offcanvas } from "react-bootstrap";
import { useLocation, useNavigate } from "react-router-dom";
import AndroidQR from "../assets/images/android-qr.png";
import IphoneQR from "../assets/images/iphone-qr.png";
import Current from "../assets/images/current.png";
import Panimg from "../assets/images/pan-img.png";
import Ashokstambh from "../assets/images/ashokstambh.png";
import KycStepper from "../components/KycStepper";
import BaseUrl from "../components/BaseUrl";

const PAN_PATTERN = /^[A-Z]{5}[0-9]{4}[A-Z]$/;

function maskEmail(value) {
  const [local, domain] = (value || "").split("@");
  if (!local || !domain) return "";

  if (local.length <= 5) {
    return `${local[0]}${"x".repeat(Math.max(local.length - 1, 1))}@${domain}`;
  }

  const visibleStart = local.slice(0, 1);
  const visibleEnd = local.slice(-4);
  const maskedLength = Math.max(local.length - visibleStart.length - visibleEnd.length, 3);
  return `${visibleStart}${"x".repeat(maskedLength)}${visibleEnd}@${domain}`;
}

function Panpage() {
  const navigate = useNavigate();
  const location = useLocation();
  // Router state doesn't survive a refresh or a direct visit to this URL,
  // so fall back to the copy stashed in sessionStorage by the email step.
  const email = location.state?.email ?? sessionStorage.getItem("kycEmail") ?? "";

  const [panNumber, setPanNumber] = useState("");
  const [panName, setPanName] = useState("");
  const [dob, setDob] = useState("");
  const [consent, setConsent] = useState(false);
  const [errors, setErrors] = useState({});
  const [showPanConfirm, setShowPanConfirm] = useState(false);
  const headingRef = useRef(null);

  useEffect(() => {
    // Reached via client-side navigation, so move focus to the heading —
    // otherwise screen reader users get no cue that a new page has loaded.
    headingRef.current?.focus();
  }, []);

  const handlePanChange = (e) => {
    setPanNumber(e.target.value.toUpperCase().replace(/[^A-Z0-9]/g, "").slice(0, 10));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const nextErrors = {};

    if (!PAN_PATTERN.test(panNumber)) {
      nextErrors.panNumber = "Please enter a valid 10-character PAN number (e.g. ABCDE1234F).";
    }

    if (!panName.trim()) {
      nextErrors.panName = "Please enter the name as per your PAN card.";
    }

    if (!dob) {
      nextErrors.dob = "Please enter your date of birth.";
    } else if (new Date(dob) > new Date()) {
      nextErrors.dob = "Date of birth cannot be in the future.";
    }

    if (!consent) {
      nextErrors.consent = "Please accept the consent checkbox to continue.";
    }

    setErrors(nextErrors);
    if (Object.keys(nextErrors).length > 0) return;

    // TODO: trigger PAN/KYC verification

    //calling api method for pan verification
    const result = await registerPan(panName, panNumber, dob);

    if(!result)
      return

    setShowPanConfirm(true);
  };

  //api call method for pan verification
  const registerPan = async (name, pan, dob) => {
    try {
      const userMasterId = sessionStorage.getItem("userMId");

      if (!userMasterId) {
        console.error("UserMasterId not found in session storage");
        return null;
      }

      const response = await fetch(
        `${BaseUrl}/Registration/pan-regis?UserMasterId=${userMasterId}`,
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json"
          },
          body: JSON.stringify({
            name: name,
            panNo: pan,
            dob: dob
          })
        }
      );

      const result = await response.json();

      if (!response.ok) {
        console.error("PAN registration failed:", result);
        setErrors({panNumber:result.message})
        return null;
      }

      console.log("PAN registration successful:", result);
      return result;
    } catch (error) {
      console.error("PAN registration error:", error);
      return null;
    }
  };

  const handleContinue = () => {
    setShowPanConfirm(false);
    // TODO: proceed to the next KYC step
  };

  const maskedEmail = maskEmail(email);
  const maskedPan = `XXXXX${panNumber.slice(-5)}`;

  return (
    <>
      <main>
        <section className="content-area">
          <Container className="kyc-stepper-wrap">
            <KycStepper currentStep={1} />
          </Container>

          <Container>
            <div className="lft-content-box">
              <h1>Complete Your KYC Verification</h1>
              <div className="kyc-info">
                <i className="fa-regular fa-circle-check" aria-hidden="true"></i>
                <p><span>Secure Verification</span> Your data is encrypted and securely stored.</p>
              </div>
              <div className="kyc-info">
                <img src={Current} alt="Current Icon" />
                <p><span>Instant Processing</span> Most verifications are completed in under 2 minutes.</p>
              </div>

              <div className="panimg">
                <img src={Panimg} alt="PAN Image" />
              </div>

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

            <div className="rgt-content-box">
              <button
                type="button"
                className="back-link"
                onClick={() => navigate(-1)}
              >
                <i className="fa-solid fa-arrow-left" aria-hidden="true"></i>
                <span>Back</span>
              </button>

              <div className="get-started-box">
                <h2 ref={headingRef} tabIndex={-1} className="step-heading">
                  PAN Number Details
                </h2>

                <form noValidate onSubmit={handleSubmit} aria-label="PAN number details">
                  {maskedEmail && (
                    <div className="static-field">
                      <span>Email ID:</span>
                      <p>{maskedEmail}</p>
                    </div>
                  )}

                  <div className="form-group">
                    <label htmlFor="panNumber">Enter Your PAN Number</label>
                    <input
                      type="text"
                      id="panNumber"
                      name="panNumber"
                      autoComplete="off"
                      maxLength={10}
                      placeholder="Enter PAN Number"
                      value={panNumber}
                      onChange={handlePanChange}
                      aria-required="true"
                      aria-describedby={errors.panNumber ? "panNumberError" : undefined}
                      aria-invalid={!!errors.panNumber}
                    />
                    {errors.panNumber && (
                      <p className="form-error" role="alert" id="panNumberError">
                        {errors.panNumber}
                      </p>
                    )}
                  </div>

                  <div className="form-group">
                    <label htmlFor="panName">Name (As per PAN card)</label>
                    <input
                      type="text"
                      id="panName"
                      name="panName"
                      autoComplete="name"
                      placeholder="Enter PAN Name"
                      value={panName}
                      onChange={(e) => setPanName(e.target.value)}
                      aria-required="true"
                      aria-describedby={errors.panName ? "panNameError" : undefined}
                      aria-invalid={!!errors.panName}
                    />
                    {errors.panName && (
                      <p className="form-error" role="alert" id="panNameError">
                        {errors.panName}
                      </p>
                    )}
                  </div>

                  <div className="form-group">
                    <label htmlFor="dob">Date Of Birth</label>
                    <input
                      type="date"
                      id="dob"
                      name="dob"
                      autoComplete="bday"
                      max={new Date().toISOString().split("T")[0]}
                      value={dob}
                      onChange={(e) => setDob(e.target.value)}
                      aria-required="true"
                      aria-describedby={errors.dob ? "dobError" : undefined}
                      aria-invalid={!!errors.dob}
                    />
                    {errors.dob && (
                      <p className="form-error" role="alert" id="dobError">
                        {errors.dob}
                      </p>
                    )}
                  </div>

                  <button type="submit" className="btn-gradient">
                    Proceed
                    <i className="fa-solid fa-arrow-right" aria-hidden="true"></i>
                  </button>
                </form>
              </div>

              <div className="consent-bx mt-4">
                <input
                  type="checkbox"
                  id="panConsentCheck"
                  name="panConsentCheck"
                  checked={consent}
                  onChange={(e) => setConsent(e.target.checked)}
                  aria-required="true"
                  aria-describedby={errors.consent ? "panConsentError" : undefined}
                  aria-invalid={!!errors.consent}
                />
                <label htmlFor="panConsentCheck">
                  I, Holder of this PAN gives my unconditional &amp; absolute consent to
                  fetch my KYC and related information (such as Name, PAN, Date of Birth,
                  Address, Mobile No., Email Address, KYC Documents, etc.) from SEBI
                  appointed Digi Locker by the Registered intermediary{" "}
                  <strong>Cazaayan Technologies Pvt. Ltd.</strong> to
                  authenticate my identity for opening my Trading &amp; DEMAT account.
                </label>
              </div>
              {errors.consent && (
                <p className="form-error" role="alert" id="panConsentError">
                  {errors.consent}
                </p>
              )}
            </div>
          </Container>
        </section>
      </main>

      <Offcanvas
        show={showPanConfirm}
        onHide={() => setShowPanConfirm(false)}
        placement="end"
        className="pan-confirm-panel"
      >
        <Offcanvas.Header closeButton closeLabel="Close">
          <Offcanvas.Title>Name as per PAN</Offcanvas.Title>
        </Offcanvas.Header>
        <Offcanvas.Body>
          <div className="pan-preview-card">
            <div>
              <span className="pan-preview-label">Name</span>
              <p className="pan-preview-value">{panName}</p>
            </div>
            <div>
              <span className="pan-preview-label">Permanent Account Number</span>
              <p className="pan-preview-value">{maskedPan}</p>
            </div>
            <div className="pan-preview-chip" aria-hidden="true">
              <img src={Ashokstambh} alt="Ashok Stambh" />
            </div>
          </div>

          <div>
            <div className="pan-info-note">
              <i className="fa-solid fa-circle-info" aria-hidden="true"></i>
              <p>
                Please confirm that the name matches your official PAN records to proceed with
                the KYC verification smoothly.
              </p>
            </div>

            <button type="button" className="btn-gradient" onClick={handleContinue}>
              Continue
              <i className="fa-solid fa-arrow-right" aria-hidden="true"></i>
            </button>
          </div>
        </Offcanvas.Body>
      </Offcanvas>
    </>
  );
}

export default Panpage;
