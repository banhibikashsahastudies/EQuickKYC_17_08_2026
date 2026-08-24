import { useEffect, useRef, useState } from "react";
import { Container } from "react-bootstrap";
import { useNavigate } from "react-router-dom";
import AccountOpeningPromo from "../components/AccountOpeningPromo";
import BaseUrl from "../components/BaseUrl";

const OTP_LENGTH = 6;
const RESEND_SECONDS = 30;

function Mobilepage() {
  const navigate = useNavigate();

  const [step, setStep] = useState("mobile");
  const [mobileNumber, setMobileNumber] = useState("");
  const [consent, setConsent] = useState(false);
  const [error, setError] = useState("");

  const [otpDigits, setOtpDigits] = useState(Array(OTP_LENGTH).fill(""));
  const [otpError, setOtpError] = useState("");
  const [resendTimer, setResendTimer] = useState(RESEND_SECONDS);

  const otpInputRefs = useRef([]);
  const stepHeadingRef = useRef(null);

  // Check whether mobile number already exists in session
  const handleView = () => {
    const savedMobile = sessionStorage.getItem("Phone");
    const AccountType = sessionStorage.getItem('AccountType');
    
    if (savedMobile && AccountType) {
      setMobileNumber(savedMobile);
      setStep("otp");
      setOtpDigits(Array(OTP_LENGTH).fill(""));
      setOtpError("");
      setResendTimer(RESEND_SECONDS);
    } else {
      setMobileNumber("");
      setStep("mobile");
    }
  };

  useEffect(() => {
    handleView();
  }, []);

  const handleMobileChange = (e) => {
    setMobileNumber(e.target.value.replace(/\D/g, "").slice(0, 10));
  };

  const handleRequestOtp = async (e) => {
    e.preventDefault();
    
    if (!/^[6-9]\d{9}$/.test(mobileNumber)) {
      setError("Please enter a valid 10-digit mobile number.");
      return;
    }

    if (!consent) {
      setError("Please accept the consent checkbox to continue.");
      return;
    }

    setError("");
    //redirection check: if account not chosen 
    if(!sessionStorage.getItem('AccountType'))
    { 
      sessionStorage.setItem('Phone',mobileNumber);
        navigate("/account_type");
    }else{
      sessionStorage.setItem('Last Page', 'Enter Phone')
    }

    //if already chosen 
    setStep("otp");
    const response = await sendMobileOTP(mobileNumber);

    if (!response) {
      return;
    }
    console.log('response:',response.data)

    // Store mobile number for the registration flow
    sessionStorage.setItem("Phone", mobileNumber);

    setOtpDigits(Array(OTP_LENGTH).fill(""));
    setOtpError("");
    setResendTimer(RESEND_SECONDS);
  };

  // API call for OTP request
  const sendMobileOTP = async (mobile) => {
    alert('sending mobile otp')
    try {
      const response = await fetch(`${BaseUrl}/Registration/send-mobile-otp`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          mobile: mobile
        })
      });

      const result = await response.json();

      if (!response.ok) {
        setError(result.message || "Unable to send OTP.");
        return null;
      }
      return result;
    } catch (error) {
      console.error("Send OTP error:", error);
      setError("Unable to send OTP. Please try again.");
      return null;
    }
  };

  useEffect(() => {
    stepHeadingRef.current?.focus();
  }, [step]);

  useEffect(() => {
    if (step !== "otp" || resendTimer === 0) {
      return undefined;
    }

    const timeoutId = setTimeout(() => {
      setResendTimer((t) => t - 1);
    }, 1000);

    return () => clearTimeout(timeoutId);
  }, [step, resendTimer]);

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
    const pasted = e.clipboardData
      .getData("text")
      .replace(/\D/g, "")
      .slice(0, OTP_LENGTH);

    if (!pasted) {
      return;
    }

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

    const otp = otpDigits.join("");

    const result = await verifyMobileOTP(mobileNumber, otp);

    if (!result) {
      return;
    }

    //delete account type and phone session data
    sessionStorage.removeItem('Phone')
    sessionStorage.removeItem('AccountType')
    navigate("/email");
  };

  // API call for verifying mobile OTP
  const verifyMobileOTP = async (mobile, otp) => {
    try {
      const response = await fetch(`${BaseUrl}/Registration/verify-mobile-otp`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json"
        },
        body: JSON.stringify({
          mobile: mobile,
          otp: otp,
          flag: sessionStorage.getItem('AccountType')??'Individual'
        })
      });

      const result = await response.json();

      if (!response.ok) {
        setOtpError(result.message || "Invalid OTP.");
        return null;
      }

      console.log("OTP verified:", result);

      // Store registration details
      sessionStorage.setItem("regId", result.data.registrationId);
      sessionStorage.setItem("userMId", result.data.userMasterId);

      return result;
    } catch (error) {
      console.error("Verify OTP error:", error);
      setOtpError("Unable to verify OTP. Please try again.");
      return null;
    }
  };

  const handleResendOtp = async () => {
    if (resendTimer > 0) {
      return;
    }

    const response = await sendMobileOTP(mobileNumber);

    if (!response) {
      return;
    }

    setOtpDigits(Array(OTP_LENGTH).fill(""));
    setOtpError("");
    setResendTimer(RESEND_SECONDS);
    otpInputRefs.current[0]?.focus();
  };

  const handleBack = () => {
    if (step === "otp") {

      //check what was the last page and hence clear that data
      if(sessionStorage.getItem('Last Page') === 'Account Type'){
        sessionStorage.removeItem('AccountType')
        navigate('/account_type')
      }else if(sessionStorage.getItem('Last Page') === 'Enter Phone'){
        sessionStorage.removeItem('Phone')
      } 

      setStep("mobile");
      return;
    }

    navigate(-1);
  };

  const maskedMobile = `xxxxxx${mobileNumber.slice(-4)}`;

  const consentBox = (
    <div className="consent-bx">
      <input
        type="checkbox"
        id="consentCheck"
        name="consentCheck"
        checked={consent}
        onChange={(e) => setConsent(e.target.checked)}
        aria-required="true"
      />

      <label htmlFor="consentCheck">
        By clicking Verify, you agree to receive important updates from{" "}
        <strong>Cazaayan Technologies Pvt. Ltd.</strong> over Whatsapp, RCS,
        RBM, and SMS concerning your Trading and Demat Account.
      </label>
    </div>
  );

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
                <i
                  className="fa-solid fa-arrow-left"
                  aria-hidden="true"
                ></i>
                <span>Back</span>
              </button>

              {step === "mobile" ? (
                <div className="get-started-box">
                  <h2
                    ref={stepHeadingRef}
                    tabIndex={-1}
                    className="step-heading"
                  >
                    Lets Get Started
                  </h2>

                  <p>Unlock your experience, instantly.</p>

                  <form
                    noValidate
                    onSubmit={handleRequestOtp}
                    aria-label="Open trading account"
                  >
                    <div className="form-group mobile-fld">
                      <label htmlFor="mobileNumber">
                        Mobile Number
                      </label>

                      <input
                        type="tel"
                        id="mobileNumber"
                        name="mobileNumber"
                        inputMode="numeric"
                        autoComplete="tel-national"
                        maxLength={10}
                        placeholder="10 - digit number"
                        value={mobileNumber}
                        onChange={handleMobileChange}
                        aria-required="true"
                        aria-describedby={error ? "formError" : undefined}
                        aria-invalid={!!error}
                      />
                    </div>

                    {error && (
                      <p
                        className="form-error"
                        role="alert"
                        id="formError"
                      >
                        {error}
                      </p>
                    )}

                    <button type="submit" className="btn-gradient">
                      Continue

                      <i
                        className="fa-solid fa-arrow-right"
                        aria-hidden="true"
                      ></i>
                    </button>

                    {consentBox}
                  </form>
                </div>
              ) : (
                <div className="get-started-box otp-verify-box">
                  <h2
                    ref={stepHeadingRef}
                    tabIndex={-1}
                    className="step-heading"
                  >
                    OTP Verify
                  </h2>

                  <p>
                    Please enter the 6-digit OTP sent to{" "}
                    <strong>{maskedMobile}</strong>
                  </p>

                  <form
                    noValidate
                    onSubmit={handleVerifyOtp}
                    aria-label="Verify OTP"
                  >
                    <div
                      className="otp-inputs"
                      role="group"
                      aria-label="6-digit OTP"
                      onPaste={handleOtpPaste}
                    >
                      {otpDigits.map((digit, index) => (
                        <input
                          key={index}
                          ref={(el) => {
                            otpInputRefs.current[index] = el;
                          }}
                          type="text"
                          inputMode="numeric"
                          autoComplete="one-time-code"
                          maxLength={1}
                          value={digit}
                          onChange={(e) =>
                            handleOtpChange(index, e.target.value)
                          }
                          onKeyDown={(e) =>
                            handleOtpKeyDown(index, e)
                          }
                          aria-label={`OTP digit ${index + 1}`}
                          aria-required="true"
                          aria-describedby={
                            otpError ? "otpFormError" : undefined
                          }
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
                      <p
                        className="form-error"
                        role="alert"
                        id="otpFormError"
                      >
                        {otpError}
                      </p>
                    )}

                    <button type="submit" className="btn-gradient">
                      Verify OTP

                      <i
                        className="fa-solid fa-arrow-right"
                        aria-hidden="true"
                      ></i>
                    </button>

                    {consentBox}
                  </form>
                </div>
              )}
            </div>
          </Container>
        </section>
      </main>
    </>
  );
}

export default Mobilepage;