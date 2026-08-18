import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

interface WorkflowRequest {
    id: number;
    title: string;
    status: string;
    priority: string;
    createdAt: string;
}

function WorkflowRequestsPage() {
    const navigate = useNavigate();
    const [requests, setRequests] = useState<WorkflowRequest[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadRequests = async () => {
            try {
                const response = await api.get("/WorkflowRequests/my");
                setRequests(response.data);
            } catch (err) {
                console.error(err);
                setError("Failed to load workflow requests.");
            } finally {
                setLoading(false);
            }
        };

        loadRequests();
    }, []);

    if (loading) {
        return <p>Loading workflow requests...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div className="dashboard-page">
            <div className="page-header">
                <div>
                    <h1>Workflow Requests</h1>
                    <p>View and manage your workflow requests.</p>
                </div>

                <button onClick={() => navigate("/requests/new")}>
                    + New Request
                </button>
            </div>

            <div className="dashboard-section">
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
                            {requests.map((request) => (
                                <tr
                                    key={request.id}
                                    onClick={() => navigate(`/requests/${request.id}`)}
                                    className="request-row"
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

                    {requests.length === 0 && (
                        <p className="empty-state">
                            No workflow requests found.
                        </p>
                    )}
                </div>
            </div>
        </div>
    );
}

export default WorkflowRequestsPage;