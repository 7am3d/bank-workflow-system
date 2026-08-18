import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { getDashboard, type DashboardData } from "../services/dashboardService";

function DashboardPage() {
    const navigate = useNavigate();
    const [dashboard, setDashboard] = useState<DashboardData | null>(null);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadDashboard = async () => {
            try {
                const data = await getDashboard();
                setDashboard(data);
            } catch {
                setError("Failed to load dashboard data.");
            } finally {
                setLoading(false);
            }
        };

        loadDashboard();
    }, []);

    if (loading) {
        return <p>Loading dashboard...</p>;
    }

    if (error || !dashboard) {
        return <p>{error || "Failed to load dashboard data."}</p>;
    }

    return (
        <div className="dashboard-page">
            <div className="page-header">
                <h1>Dashboard</h1>
                <p>Welcome to your BankWorkflow dashboard.</p>
            </div>

            <div className="stats-grid">
                <div className="stat-card">
                    <span className="stat-label">My Requests</span>
                    <span className="stat-value">
                        {dashboard.myRequests}
                    </span>
                </div>

                <div className="stat-card">
                    <span className="stat-label">Pending</span>
                    <span className="stat-value">
                        {dashboard.myPendingRequests}
                    </span>
                </div>

                <div className="stat-card">
                    <span className="stat-label">Approved</span>
                    <span className="stat-value">
                        {dashboard.myApprovedRequests}
                    </span>
                </div>

                <div className="stat-card">
                    <span className="stat-label">Rejected</span>
                    <span className="stat-value">
                        {dashboard.myRejectedRequests}
                    </span>
                </div>
            </div>

            {/* Recent Requests */}
            <div className="dashboard-section">
                <div className="section-header">
                    <div>
                        <h2>Recent Requests</h2>
                        <p>Your latest workflow requests.</p>
                    </div>
                </div>

                {dashboard.recentRequests.length === 0 ? (
                    <p className="empty-state">
                        No recent requests.
                    </p>
                ) : (
                    <div className="table-container">
                        <table className="requests-table">
                            <thead>
                                <tr>
                                    <th>Request</th>
                                    <th>Status</th>
                                    <th>Priority</th>
                                    <th>Created</th>
                                </tr>
                            </thead>

                            <tbody>
                                {dashboard.recentRequests.map((request) => (
                                    <tr
                                        key={request.id}
                                        onClick={() =>
                                            navigate(`/requests/${request.id}`)
                                        }
                                        style={{ cursor: "pointer" }}
                                    >
                                        <td>
                                            <strong>{request.title}</strong>
                                        </td>

                                        <td>
                                            <span
                                                className={`status-badge status-${request.status.toLowerCase()}`}
                                            >
                                                {request.status}
                                            </span>
                                        </td>

                                        <td>
                                            <span
                                                className={`priority-badge priority-${request.priority.toLowerCase()}`}
                                            >
                                                {request.priority}
                                            </span>
                                        </td>

                                        <td>
                                            {new Date(
                                                request.createdAt
                                            ).toLocaleDateString()}
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>

            {/* Pending Approvals */}
            {dashboard.pendingApprovalRequests.length > 0 && (
                <div className="dashboard-section">
                    <div className="section-header">
                        <div>
                            <h2>Pending Approvals</h2>
                            <p>Requests waiting for your approval.</p>
                        </div>

                        {dashboard.pendingApprovals > 5 && (
                            <button
                                className="view-all-button"
                                onClick={() => navigate("/pending-approvals")}
                            >
                                View All
                            </button>
                        )}
                    </div>

                    <div className="table-container">
                        <table className="requests-table">
                            <thead>
                                <tr>
                                    <th>Request</th>
                                    <th>Requested By</th>
                                    <th>Priority</th>
                                    <th>Created</th>
                                </tr>
                            </thead>

                            <tbody>
                                {dashboard.pendingApprovalRequests.map(
                                    (request) => (
                                        <tr
                                            key={request.id}
                                            onClick={() =>
                                                navigate(
                                                    `/requests/${request.id}`
                                                )
                                            }
                                            style={{ cursor: "pointer" }}
                                        >
                                            <td>
                                                <strong>
                                                    {request.title}
                                                </strong>
                                            </td>

                                            <td>
                                                {request.requestedBy}
                                            </td>

                                            <td>
                                                <span
                                                    className={`priority-badge priority-${request.priority.toLowerCase()}`}
                                                >
                                                    {request.priority}
                                                </span>
                                            </td>

                                            <td>
                                                {new Date(
                                                    request.createdAt
                                                ).toLocaleDateString()}
                                            </td>
                                        </tr>
                                    )
                                )}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}
        </div>
    );
}

export default DashboardPage;