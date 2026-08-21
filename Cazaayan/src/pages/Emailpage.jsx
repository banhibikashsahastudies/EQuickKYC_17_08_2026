import { useEffect, useRef, useState } from "react";
import { Container } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import AccountOpeningPromo from "../components/AccountOpeningPromo";
import BaseUrl from "../components/BaseUrl";

const OTP_LENGTH = 6;
const RESEND_SECONDS = 30;

function maskEmail(value) {
  const [local, domain] = value.split("@");
  if (!local || !domain) return value;
  const visible = local.slice(0, 2);
  const masked = "*".repeat(Math.max(local.length - visible.length, 3));
  return `${visible}${masked}@${domain}`;
}

function Emailpage() {
  const navigate = useNavigate();
  const [step, setStep] = useState("email"); // "email" | "otp"
  const [email, setEmail] = useState("");
  const [consent, setConsent] = useState(false);
  const [error, setError] = useState("");

  const [otpDigits, setOtpDigits] = useState(Array(OTP_LENGTH).fill(""));
  const [otpError, setOtpError] = useState("");
  const [resendTimer, setResendTimer] = useState(RESEND_SECONDS);
  const otpInputRefs = useRef([]);
  const stepHeadingRef = useRef(null);

  useEffect(() => {
    // This page is reached via client-side navigation and also swaps steps
    // in place, so move focus to the current step's heading whenever it
    // changes — otherwise screen reader users get no indication that the
    // page/section has changed.
    stepHeadingRef.current?.focus();
  }, [step]);

  useEffect(() => {
    if (step !== "otp" || resendTimer === 0) return undefined;
    const timeoutId = setTimeout(() => setResendTimer((t) => t - 1), 1000);
    return () => clearTimeout(timeoutId);
  }, [step, resendTimer]);

  const handleEmailChange = (e) => {
    setEmail(e.target.value);
  };

  const handleRequestOtp = async (e) => {
    e.preventDefault();

    if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email)) {
      setError("Please enter a valid email address.");
      return;
    }

    if (!consent) {
      setError("Please accept the consent checkbox to continue.");
      return;
    }

    setError("");

    // TODO: trigger email OTP request
    const response = await sendEmailOTP(email);

    console.log('response email:',response.data);

    setOtpDigits(Array(OTP_LENGTH).fill(""));
    setOtpError("");
    setResendTimer(RESEND_SECONDS);
    setStep("otp");
  };



  // API Call for OTP Request
  const sendEmailOTP = async (email) => {
    
    try {
      const response = await fetch(`${BaseUrl}/Registration/send-email-otp`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json'
        },
        body: JSON.stringify({
          email: email
        })
      });

      const result = await response.json();

      if (!response.ok) {
        setError(result.message);
      }

      return result;
    } catch (error) {
      console.error('Send OTP error:', error);
      throw error;
    }
  };

  const handleOtpChange = (index, value) => {
    const digit = value.replace(/\D/g, "").slice(-1);

    setOtpDigits((prev) => {
      const next = [...prev];
      next[index] = digit;
      return next;
    });

    if (digit && index < OTP_LENGTH - 1) {
      otpInputRefs.current[index + 1]?.focus();
    }
  };

  const handleOtpKeyDown = (index, e) => {
    if (e.key === "Backspace" && !otpDigits[index] && index > 0) {
      otpInputRefs.current[index - 1]?.focus();
    }
  };

  const handleOtpPaste = (e) => {
    const pasted = e.clipboardData.getData("text").replace(/\D/g, "").slice(0, OTP_LENGTH);
    if (!pasted) return;
    e.preventDefault();

    setOtpDigits((prev) => {
      const next = [...prev];
      pasted.split("").forEach((digit, i) => {
        next[i] = digit;
      });
      return next;
    });

    const lastIndex = Math.min(pasted.length, OTP_LENGTH) - 1;
    otpInputRefs.current[lastIndex]?.focus();
  };

  const handleVerifyOtp = async (e) => {
    e.preventDefault();

    if (otpDigits.join("").length < OTP_LENGTH) {
      setOtpError("Please enter the 6-digit OTP.");
      return;
    }

    if (!consent) {
      setOtpError("Please accept the consent checkbox to continue.");
      return;
    }

    setOtpError("");
    // TODO: trigger email OTP verification
    // Persisted alongside the router state so the email still shows up on
    // the PAN page after a refresh or a direct visit during this session.

    //calling email verify function
    let otp = otpDigits.join("")
    const result = await verifyEmailOTP(email, otp);

    if (!result) {
      return;
    }

    sessionStorage.setItem("kycEmail", email);
    navigate("/pan", { state: { email } });
  };

  //API call method for verifying email otp
  const verifyEmailOTP = async (email, otp) => {
    try {
      const response = await fetch(`${BaseUrl}/Registration/verify-email-otp`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          email: email,
          otp: otp,
          usermasterId: sessionStorage.getItem('userMId')
        })
      });

      const result = await response.json();

      if (!response.ok) {
        setOtpError(result.message || "Invalid OTP.");
        return null;
      }

      return result;
    } catch (error) {
      console.error("Verify OTP error:", error);
      setOtpError("Unable to verify OTP. Please try again.");
      return null;
    }
  };

  const handleResendOtp = () => {
    if (resendTimer > 0) return;

    // TODO: trigger email OTP resend
    setOtpDigits(Array(OTP_LENGTH).fill(""));
    setOtpError("");
    setResendTimer(RESEND_SECONDS);
    otpInputRefs.current[0]?.focus();
  };

  const handleBack = () => {
    if (step === "otp") {
      setStep("email");
      return;
    }
    navigate(-1);
  };

  const maskedEmail = maskEmail(email);

  return (
    <>
      <main>
        <section className="content-area">
          <Container>
            <AccountOpeningPromo />

            <div className="rgt-content-box">
              <button
                type="button"
                className="back-link"
                onClick={handleBack}
              >
                <i className="fa-solid fa-arrow-left" aria-hidden="true"></i>
                <span>Back</span>
              </button>

              {step === "email" ? (
                <div className="get-started-box">
                  <h2 ref={stepHeadingRef} tabIndex={-1} className="step-heading">
                    Enter Your Email
                  </h2>
                  <p>Lets add an Email ID you would like to associate with this account</p>

                  <form noValidate onSubmit={handleRequestOtp} aria-label="Add email to account">
                    <div className="form-group email-fld">
                      <label htmlFor="emailId">Email ID</label>
                      <input
                        type="email"
                        id="emailId"
                        name="emailId"
                        autoComplete="email"
                        placeholder="Enter your Email ID"
                        value={email}
                        onChange={handleEmailChange}
                        aria-required="true"
                        aria-describedby={error ? "emailFormError" : "emailHint"}
                        aria-invalid={!!error}
                      />
                      <p className="field-hint" id="emailHint">
                        Your entered email address will receive an 6 digit OTP
                      </p>
                    </div>

                    {error && (
                      <p className="form-error" role="alert" id="emailFormError">
                        {error}
                      </p>
                    )}

                    <button type="submit" className="btn-gradient">
                      Request OTP
                      <i className="fa-solid fa-arrow-right" aria-hidden="true"></i>
                    </button>
                  </form>
                </div>
              ) : (
                <div className="get-started-box otp-verify-box">
                  <h2 ref={stepHeadingRef} tabIndex={-1} className="step-heading">
                    Verify Email ID
                  </h2>
                  <p>
                    Please enter the 6-digit OTP sent to <strong>{maskedEmail}</strong>
                  </p>

                  <form noValidate onSubmit={handleVerifyOtp} aria-label="Verify email OTP">
                    <div
                      className="otp-inputs"
                      role="group"
                      aria-label="6-digit OTP"
                      onPaste={handleOtpPaste}
                    >
                      {otpDigits.map((digit, index) => (
                        <input
                          key={index}
                          ref={(el) => (otpInputRefs.current[index] = el)}
                          type="text"
                          inputMode="numeric"
                          autoComplete="one-time-code"
                          maxLength={1}
                          value={digit}
                          onChange={(e) => handleOtpChange(index, e.target.value)}
                          onKeyDown={(e) => handleOtpKeyDown(index, e)}
                          aria-label={`OTP digit ${index + 1}`}
                          aria-required="true"
                          aria-describedby={otpError ? "otpFormError" : undefined}
                          aria-invalid={!!otpError}
                        />
                      ))}
                    </div>

                    <div className="otp-resend-row">
                      <span>
                        {resendTimer > 0
                          ? `The code is on its way, ${resendTimer}s`
                          : "Didn't receive the code?"}
                      </span>
                      <button
                        type="button"
                        className="resend-link"
                        onClick={handleResendOtp}
                        disabled={resendTimer > 0}
                      >
                        Resend
                      </button>
                    </div>

                    {otpError && (
                      <p className="form-error" role="alert" id="otpFormError">
                        {otpError}
                      </p>
                    )}

                    <button type="submit" className="btn-gradient">
                      Verify OTP
                      <i className="fa-solid fa-arrow-right" aria-hidden="true"></i>
                    </button>
                  </form>
                </div>
              )}

              <div className="consent-bx mt-4">
                <input
                  type="checkbox"
                  id="emailConsentCheck"
                  name="emailConsentCheck"
                  checked={consent}
                  onChange={(e) => setConsent(e.target.checked)}
                  aria-required="true"
                />
                <label htmlFor="emailConsentCheck">
                  By clicking Verify, you agree to receive important updates from{" "}
                  <strong>Cazaayan Technologies Pvt. Ltd.</strong> over
                  Whatsapp, RCS, RBM, and SMS concerning your Trading and Demat Account.
                </label>
              </div>
            </div>
          </Container>
        </section>
      </main>
    </>
  );
}

export default Emailpage;
