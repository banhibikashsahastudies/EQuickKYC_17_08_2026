import { useState } from "react";
import BaseUrl from "../components/BaseUrl";
import "../assets/css/banhiStyle.css";

function BankSearch() {
    const [bankName, setBankName] = useState("");
    const [ifsc, setIfsc] = useState("");
    const [branchName, setBranchName] = useState("");

    const [searchBankName, setSearchBankName] = useState(false);
    const [searchIfsc, setSearchIfsc] = useState(false);
    const [searchBranchName, setSearchBranchName] = useState(false);

    const [banks, setBanks] = useState([]);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");
    const [success, setSuccess] = useState("");

    const handleSearch = async () => {
        setError("");
        setSuccess("");
        setBanks([]);

        if (!searchBankName && !searchIfsc && !searchBranchName) {
            setError("Please select at least one search field.");
            return;
        }

        if (searchBankName && !bankName.trim()) {
            setError("Please enter Bank Name.");
            return;
        }

        if (searchIfsc && !ifsc.trim()) {
            setError("Please enter IFSC.");
            return;
        }

        if (searchBranchName && !branchName.trim()) {
            setError("Please enter Branch Name.");
            return;
        }

        try {
            setLoading(true);

            const params = new URLSearchParams();

            if (searchBankName) {
                params.append("name", bankName.trim());
            }

            if (searchIfsc) {
                params.append("ifsc", ifsc.trim());
            }

            if (searchBranchName) {
                params.append("branchName", branchName.trim());
            }

            const response = await fetch(
                `${BaseUrl}/BankMaster/GetBankByParams?${params.toString()}`,
                {
                    method: "GET"
                }
            );

            const result = await response.json();

            console.log("Bank search response:", result);

            if (!response.ok) {
                setError(result.message || "Unable to search banks.");
                return;
            }

            const data = result.data || [];

            setBanks(data);

            if (data.length === 0) {
                setSuccess("No banks found for the selected search criteria.");
            } else {
                setSuccess(`${data.length} bank record(s) found.`);
            }

        } catch (error) {
            console.error("Bank search error:", error);
            setError("Unable to connect to the server. Please try again.");
        } finally {
            setLoading(false);
        }
    };

    const handleClear = () => {
        setBankName("");
        setIfsc("");
        setBranchName("");

        setSearchBankName(false);
        setSearchIfsc(false);
        setSearchBranchName(false);

        setBanks([]);
        setError("");
        setSuccess("");
    };

    return (
        <div className="admin-page">

            <div className="admin-container">

                <div className="admin-header">
                    <div>
                        <h1>Bank Search</h1>
                        <p>
                            Search banks using Bank Name, IFSC, or Branch Name.
                        </p>
                    </div>
                </div>

                <div className="upload-card">

                    <div className="upload-header">
                        <div>
                            <h2>Search Bank</h2>
                            <p>
                                Select the fields you want to use for searching.
                            </p>
                        </div>
                    </div>

                    <div className="bank-search-content">

                        <div className="bank-search-field">

                            <div className="search-field-header">

                                <label>
                                    <input
                                        type="checkbox"
                                        checked={searchBankName}
                                        onChange={(e) => {
                                            setSearchBankName(e.target.checked);

                                            if (!e.target.checked) {
                                                setBankName("");
                                            }
                                        }}
                                    />

                                    <span>Search by Bank Name</span>
                                </label>

                            </div>

                            <input
                                type="text"
                                value={bankName}
                                onChange={(e) => setBankName(e.target.value)}
                                placeholder="Enter Bank Name"
                                disabled={!searchBankName}
                            />

                        </div>

                        <div className="bank-search-field">

                            <div className="search-field-header">

                                <label>
                                    <input
                                        type="checkbox"
                                        checked={searchIfsc}
                                        onChange={(e) => {
                                            setSearchIfsc(e.target.checked);

                                            if (!e.target.checked) {
                                                setIfsc("");
                                            }
                                        }}
                                    />

                                    <span>Search by IFSC</span>
                                </label>

                            </div>

                            <input
                                type="text"
                                value={ifsc}
                                onChange={(e) => setIfsc(e.target.value.toUpperCase())}
                                placeholder="Enter IFSC Code"
                                disabled={!searchIfsc}
                            />

                        </div>

                        <div className="bank-search-field">

                            <div className="search-field-header">

                                <label>
                                    <input
                                        type="checkbox"
                                        checked={searchBranchName}
                                        onChange={(e) => {
                                            setSearchBranchName(e.target.checked);

                                            if (!e.target.checked) {
                                                setBranchName("");
                                            }
                                        }}
                                    />

                                    <span>Search by Branch Name</span>
                                </label>

                            </div>

                            <input
                                type="text"
                                value={branchName}
                                onChange={(e) => setBranchName(e.target.value)}
                                placeholder="Enter Branch Name"
                                disabled={!searchBranchName}
                            />

                        </div>

                        <div className="upload-actions">

                            <button
                                type="button"
                                className="upload-btn"
                                onClick={handleSearch}
                                disabled={loading}
                            >
                                {loading ? (
                                    <>
                                        <i className="fa-solid fa-spinner fa-spin"></i>
                                        Searching...
                                    </>
                                ) : (
                                    <>
                                        <i className="fa-solid fa-magnifying-glass"></i>
                                        Search
                                    </>
                                )}
                            </button>

                            <button
                                type="button"
                                className="clear-btn"
                                onClick={handleClear}
                            >
                                <i className="fa-solid fa-xmark"></i>
                                Clear
                            </button>

                        </div>

                    </div>

                </div>

                {error && (
                    <div className="admin-error">

                        <i className="fa-solid fa-circle-exclamation"></i>

                        <div>
                            <strong>Search Error</strong>
                            <span>{error}</span>
                        </div>

                    </div>
                )}

                {success && !error && (
                    <div className="admin-success">

                        <i className="fa-solid fa-circle-check"></i>

                        <div>
                            <strong>Search Result</strong>
                            <span>{success}</span>
                        </div>

                    </div>
                )}

                <div className="table-card">

                    <div className="table-header">

                        <div>
                            <h2>Bank Records</h2>

                            <span>
                                {banks.length} record
                                {banks.length !== 1 ? "s" : ""}
                            </span>
                        </div>

                    </div>

                    {loading ? (

                        <div className="admin-loading">

                            <div className="loader"></div>

                            <p>
                                Searching bank records...
                            </p>

                        </div>

                    ) : banks.length === 0 ? (

                        <div className="no-data">

                            <i className="fa-solid fa-building-columns"></i>

                            <h3>No Records</h3>

                            <p>
                                Select search criteria and search for a bank.
                            </p>

                        </div>

                    ) : (

                        <div className="table-wrapper">

                            <table className="error-table bank-table">

                                <thead>
                                    <tr>
                                        <th>Bank Name</th>
                                        <th>Branch Name</th>
                                        <th>Branch Code</th>
                                        <th>IFSC Code</th>
                                        <th>MICR Code</th>
                                        <th>Country</th>
                                        <th>State</th>
                                        <th>City</th>
                                        <th>Zip Code</th>
                                        <th>Status</th>
                                    </tr>
                                </thead>

                                <tbody>

                                    {banks.map((bank, index) => (

                                        <tr key={bank.id || index}>

                                            <td>
                                                {bank.bankName || "-"}
                                            </td>

                                            <td>
                                                {bank.branchName || "-"}
                                            </td>

                                            <td>
                                                {bank.branchCode || "-"}
                                            </td>

                                            <td>
                                                <span className="correlation-id">
                                                    {bank.ifscCode || "-"}
                                                </span>
                                            </td>

                                            <td>
                                                {bank.micrCode || "-"}
                                            </td>

                                            <td>
                                                {bank.address?.country || "-"}
                                            </td>

                                            <td>
                                                {bank.address?.state || "-"}
                                            </td>

                                            <td>
                                                {bank.address?.city || "-"}
                                            </td>

                                            <td>
                                                {bank.address?.zipCode || "-"}
                                            </td>

                                            <td>
                                                <span className="status-badge">
                                                    {bank.status ? "Active" : "Inactive"}
                                                </span>
                                            </td>

                                        </tr>

                                    ))}

                                </tbody>

                            </table>

                        </div>

                    )}

                </div>

            </div>

        </div>
    );
}

export default BankSearch;