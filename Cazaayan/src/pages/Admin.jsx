import { useEffect, useState } from "react";
import BaseUrl from "../components/BaseUrl";
import '../assets/css/banhiStyle.css';

function Admin() {
    const [errors, setErrors] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    const fetchErrors = async () => {
        try {
            setLoading(true);
            setError("");

            const response = await fetch(
                `${BaseUrl}/Admin/get-api-error-log`
            );

            const result = await response.json();
            console.log('result:', result.data);
            if (!response.ok) {
                setError(
                    result.message || "Unable to load error records."
                );
                return;
            }

            setErrors(result.data || []);
        } catch (error) {
            console.error("Error fetching error records:", error);
            setError("Unable to connect to the server.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchErrors();
    }, []);

    return (
        <div className="admin-page">
            <div className="admin-container">

                <div className="admin-header">
                    <div>
                        <h1>Admin Dashboard</h1>
                        <p>External API Error Records</p>
                    </div>

                    <button
                        type="button"
                        className="refresh-btn"
                        onClick={fetchErrors}
                    >
                        <i className="fa-solid fa-rotate-right"></i>
                        Refresh
                    </button>
                </div>

                {error && (
                    <div className="admin-error">
                        <i className="fa-solid fa-circle-exclamation"></i>
                        <span>{error}</span>
                    </div>
                )}

                {loading ? (
                    <div className="admin-loading">
                        <div className="loader"></div>
                        <p>Loading error records...</p>
                    </div>
                ) : (
                    <div className="table-card">

                        <div className="table-header">
                            <div>
                                <h2>Error Records</h2>
                                <span>
                                    {errors.length} record
                                    {errors.length !== 1 ? "s" : ""}
                                </span>
                            </div>
                        </div>

                        {errors.length === 0 ? (
                            <div className="no-data">
                                <i className="fa-solid fa-circle-check"></i>
                                <h3>No Error Records</h3>
                                <p>
                                    There are currently no external API
                                    errors to display.
                                </p>
                            </div>
                        ) : (
                            <div className="table-wrapper">
                                <table className="error-table">
                                    <thead>
                                        <tr>
                                            <th>ID</th>
                                            <th>Correlation ID</th>
                                            <th>Error Captured At</th>
                                            <th>Service</th>
                                            <th>Endpoint</th>
                                            <th>External API</th>
                                            <th>Status Code</th>
                                            <th>Error Type</th>
                                            <th>Error Message</th>
                                            <th>Client Message</th>
                                        </tr>
                                    </thead>

                                    <tbody>
                                        {errors.map((item) => (
                                            <tr key={item.id}>

                                                <td>
                                                    <span className="id-badge">
                                                        {item.id}
                                                    </span>
                                                </td>

                                                <td>
                                                    <span className="correlation-id">
                                                        {item.correlationId || "-"}
                                                    </span>
                                                </td>

                                                <td>
                                                    {item.errorCapturedAt
                                                        ? new Date(
                                                            item.errorCapturedAt
                                                        ).toLocaleString()
                                                        : "-"}
                                                </td>

                                                <td>
                                                    <strong>
                                                        {item.serviceName || "-"}
                                                    </strong>
                                                </td>

                                                <td>
                                                    <span className="endpoint">
                                                        {item.endpoint || "-"}
                                                    </span>
                                                </td>

                                                <td style={{ color: "#dc3545" }}>
                                                    {item.externalApi || "-"}
                                                </td>

                                                <td>
                                                    <span className="status-badge">
                                                        {item.httpStatusCode || "-"}
                                                    </span>
                                                </td>

                                                <td>
                                                    <span className="type-badge">
                                                        {item.errorType || "-"}
                                                    </span>
                                                </td>

                                                <td>
                                                    {item.errorMessage || "-"}
                                                </td>

                                                <td>
                                                    <span className="client-message">
                                                        {item.clientMessage || "-"}
                                                    </span>
                                                </td>

                                            </tr>
                                        ))}
                                    </tbody>
                                </table>
                            </div>
                        )}
                    </div>
                )}
            </div>
        </div>
    );
}

export default Admin;