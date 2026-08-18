import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    getPendingApprovals,
    type PendingApprovalRequest,
} from "../services/workflowRequestService";

function PendingApprovalsPage() {
    const navigate = useNavigate();

    const [requests, setRequests] = useState<PendingApprovalRequest[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");

    useEffect(() => {
        const loadRequests = async () => {
            try {
                const data = await getPendingApprovals();
                setRequests(data);
            } catch (err) {
                console.error(err);
                setError("Failed to load pending approvals.");
            } finally {
                setLoading(false);
            }
        };

        loadRequests();
    }, []);

    if (loading) {
        return <p>Loading pending approvals...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    return (
        <div className="dashboard-page">
            <div className="page-header">
                <h1>Pending Approvals</h1>
                <p>Requests waiting for your approval.</p>
            </div>

            <div className="dashboard-section">
                {requests.length === 0 ? (
                    <p className="empty-state">
                        There are no requests waiting for your approval.
                    </p>
                ) : (
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
                                {requests.map((request) => (
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
                                ))}
                            </tbody>
                        </table>
                    </div>
                )}
            </div>
        </div>
    );
}

export default PendingApprovalsPage;