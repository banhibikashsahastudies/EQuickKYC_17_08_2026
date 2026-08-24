import { useEffect, useRef, useState } from "react";
import { Container } from "react-bootstrap";
import { useNavigate } from "react-router-dom";

import Ashokstambh from "../assets/images/Ashokstambh.png";
import API_BASE_URL from "../components/BaseUrl";

function ViewPanDetails() {
    const navigate = useNavigate();
    const headingRef = useRef(null);

    const [panNumber, setPanNumber] = useState("");
    const [panFound, setPanFound] = useState(false);
    const [foundPan, setFoundPan] = useState("");
    const [foundName, setFoundName] = useState("");
    const [error, setError] = useState("");
    const [loading, setLoading] = useState(false);

    useEffect(() => {
        headingRef.current?.focus();
    }, [panFound]);

    const handlePanChange = (e) => {
        const value = e.target.value
            .toUpperCase()
            .replace(/[^A-Z0-9]/g, "")
            .slice(0, 10);

        setPanNumber(value);
        setError("");
        setPanFound(false);
        setFoundPan("");
        setFoundName("");
    };

    const verifyPan = async () => {
        try {
            const response = await fetch(
                `${API_BASE_URL}/Verification/pan-verification?panNumber=${encodeURIComponent(panNumber)}`,
                {
                    method: "GET"
                }
            );

            const result = await response.json();

            console.log("PAN verification response:", result);

            if (!response.ok) {
                setError(result.message || "Unable to verify PAN.");
                return null;
            }

            return result;
        } catch (error) {
            console.error("PAN verification error:", error);
            setError("Unable to connect to the server. Please try again.");
            return null;
        }
    };

    const handleSubmit = async (e) => {
        e.preventDefault();

        setError("");
        setPanFound(false);
        setFoundPan("");
        setFoundName("");

        if (!panNumber) {
            setError("Please enter your PAN number.");
            return;
        }

        if (!/^[A-Z]{5}[0-9]{4}[A-Z]$/.test(panNumber)) {
            setError("Please enter a valid PAN number.");
            return;
        }

        setLoading(true);

        const result = await verifyPan();

        setLoading(false);

        if (!result) {
            return;
        }

        console.log("PAN verification response:", result);

        if (result.success && result.data) {
            setFoundPan(result.data.panNo || "");
            setFoundName(result.data.name || "");
            setPanFound(true);

            sessionStorage.setItem(
                "verifiedPan",
                result.data.panNo || ""
            );

            sessionStorage.setItem(
                "verifiedPanName",
                result.data.name || ""
            );

            return;
        }

        setError(result.message || "PAN was not found.");
    };

    const handleBack = () => {
        setPanFound(false);
        setFoundPan("");
        setFoundName("");
        setError("");
    };

    const maskedPan = foundPan
        ? `${foundPan.slice(0, 2)}****${foundPan.slice(-4)}`
        : "";

    return (
        <main>
            <section className="content-area">
                <Container>
                    <div className="rgt-content-box">

                        {!panFound ? (
                            <div
                                className="get-started-box"
                                style={{
                                    maxWidth: "500px",
                                    margin: "0 auto"
                                }}
                            >
                                <h2
                                    ref={headingRef}
                                    tabIndex={-1}
                                    className="step-heading"
                                >
                                    PAN Number Check
                                </h2>

                                <p>
                                    Please enter the PAN number which you
                                    want to check.
                                </p>

                                <form
                                    noValidate
                                    onSubmit={handleSubmit}
                                    aria-label="PAN number verification"
                                >
                                    <div className="form-group">
                                        <label htmlFor="panNumber">
                                            Enter the PAN Number
                                        </label>

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
                                            aria-invalid={!!error}
                                        />
                                    </div>

                                    {error && (
                                        <p
                                            className="form-error"
                                            role="alert"
                                        >
                                            {error}
                                        </p>
                                    )}

                                    <button
                                        type="submit"
                                        className="btn-gradient"
                                        disabled={loading}
                                    >
                                        {loading
                                            ? "Checking..."
                                            : "Search"}

                                        {!loading && (
                                            <i
                                                className="fa-solid fa-arrow-right"
                                                aria-hidden="true"
                                            ></i>
                                        )}
                                    </button>
                                </form>
                            </div>
                        ) : (
                            <div
                                className="get-started-box"
                                style={{
                                    maxWidth: "500px",
                                    margin: "0 auto"
                                }}
                            >
                                <h2
                                    ref={headingRef}
                                    tabIndex={-1}
                                    className="step-heading"
                                >
                                    PAN Found
                                </h2>

                                <p>
                                    The PAN number was found in our database.
                                </p>

                                <div
                                    style={{
                                        width: "100%",
                                        display: "flex",
                                        justifyContent: "center"
                                    }}
                                >
                                    <div
                                        className="pan-confirm-panel"
                                        style={{
                                            margin: "0 auto"
                                        }}
                                    >
                                        <div className="pan-preview-card">

                                            <div>
                                                <span className="pan-preview-label">
                                                    Name
                                                </span>

                                                <p className="pan-preview-value">
                                                    {foundName}
                                                </p>
                                            </div>

                                            <div>
                                                <span className="pan-preview-label">
                                                    Permanent Account Number
                                                </span>

                                                <p className="pan-preview-value">
                                                    {maskedPan}
                                                </p>
                                            </div>

                                            <div
                                                className="pan-preview-chip"
                                                aria-hidden="true"
                                            >
                                                <img
                                                    src={Ashokstambh}
                                                    alt="Ashok Stambh"
                                                />
                                            </div>

                                        </div>
                                    </div>
                                </div>

                                <button
                                    type="button"
                                    className="btn-gradient"
                                    onClick={handleBack}
                                >
                                    <i
                                        className="fa-solid fa-arrow-left"
                                        aria-hidden="true"
                                    ></i>

                                    Back
                                </button>
                            </div>
                        )}

                    </div>
                </Container>
            </section>
        </main>
    );
}

export default ViewPanDetails;