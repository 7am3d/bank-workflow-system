import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import api from "../services/api";

interface WorkflowRequest {
    id: number;
    title: string;
    description: string;
    requestType: string;
    createdBy: string;
    status: string;
    priority: string;
    currentStep: number;
    createdAt: string;
    canCurrentUserAct: boolean;
}

interface WorkflowHistory {
    id: number;
    action: string;
    previousStatus: string | null;
    newStatus: string | null;
    details: string | null;
    createdAt: string;
    performedBy: string;
}

function WorkflowRequestDetailsPage() {
    const { id } = useParams();
    const navigate = useNavigate();

    const [request, setRequest] = useState<WorkflowRequest | null>(null);
    const [history, setHistory] = useState<WorkflowHistory[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [actionLoading, setActionLoading] = useState(false);
    const [actionError, setActionError] = useState("");

    useEffect(() => {
        const loadRequest = async () => {
            try {
                const response = await api.get(`/WorkflowRequests/${id}`);
                setRequest(response.data);

                const historyResponse = await api.get(
                    `/WorkflowRequests/${id}/history`
                );
                setHistory(historyResponse.data);
            } catch (err) {
                console.error(err);
                setError("Failed to load workflow request.");
            } finally {
                setLoading(false);
            }
        };

        loadRequest();
    }, [id]);

    const handleApprove = async () => {
        try {
            setActionLoading(true);
            setActionError("");

            await api.post(`/WorkflowRequests/${id}/approve`);

            const response = await api.get(`/WorkflowRequests/${id}`);
            setRequest(response.data);

            const historyResponse = await api.get(
                `/WorkflowRequests/${id}/history`
            );
            setHistory(historyResponse.data);
        } catch (err) {
            console.error(err);
            setActionError("Failed to approve workflow request.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleReject = async () => {
        const reason = window.prompt("Enter a reason for rejecting this request:");

        if (!reason || !reason.trim()) {
            return;
        }

        try {
            setActionLoading(true);
            setActionError("");

            await api.post(`/WorkflowRequests/${id}/reject`, {
                reason: reason.trim(),
            });

            const response = await api.get(`/WorkflowRequests/${id}`);
            setRequest(response.data);

            const historyResponse = await api.get(
                `/WorkflowRequests/${id}/history`
            );
            setHistory(historyResponse.data);
        } catch (err) {
            console.error(err);
            setActionError("Failed to reject workflow request.");
        } finally {
            setActionLoading(false);
        }
    };

    if (loading) {
        return <p>Loading workflow request...</p>;
    }

    if (error) {
        return <p>{error}</p>;
    }

    if (!request) {
        return <p>Workflow request not found.</p>;
    }

    return (
        <div className="dashboard-page">
            <div className="page-header">
                <button onClick={() => navigate("/requests")}>
                    ← Back to Requests
                </button>

                <h1>{request.title}</h1>
                <p>Workflow request details</p>
            </div>

            <div className="dashboard-section">
                <h2>Request Details</h2>

                <p>
                    <strong>Description:</strong> {request.description}
                </p>

                <p>
                    <strong>Request Type:</strong> {request.requestType}
                </p>

                <p>
                    <strong>Created By:</strong> {request.createdBy}
                </p>

                <p>
                    <strong>Status:</strong> {request.status}
                </p>

                <p>
                    <strong>Priority:</strong> {request.priority}
                </p>

                <p>
                    <strong>Current Step:</strong> {request.currentStep}
                </p>

                <p>
                    <strong>Created:</strong>{" "}
                    {new Date(request.createdAt).toLocaleDateString()}
                </p>
            </div>

            {request.status === "Pending" && request.canCurrentUserAct && (
                <div className="dashboard-section">
                    <h2>Workflow Actions</h2>

                    {actionError && (
                        <p className="error-message">
                            {actionError}
                        </p>
                    )}

                    <div className="workflow-actions">
                        <button
                            onClick={handleApprove}
                            disabled={actionLoading}
                        >
                            {actionLoading ? "Processing..." : "Approve"}
                        </button>

                        <button
                            onClick={handleReject}
                            disabled={actionLoading}
                        >
                            {actionLoading ? "Processing..." : "Reject"}
                        </button>
                    </div>
                </div>
            )}

            <div className="dashboard-section">
                <div className="section-header">
                    <h2>Workflow History</h2>
                    <p>Activity and status changes for this request.</p>
                </div>

                <div className="history-list">
                    {history.map((item) => (
                        <div className="history-item" key={item.id}>
                            <div className="history-content">
                                <h3>{item.action}</h3>

                                {item.previousStatus && item.previousStatus !== item.newStatus ? (
                                    <p>
                                        {item.previousStatus} → {item.newStatus}
                                    </p>
                                ) : item.previousStatus === null ? (
                                    <p>{item.newStatus}</p>
                                ) : null}
                                {item.details && (
                                    <p>{item.details}</p>
                                )}

                                <small>
                                    {item.performedBy} •{" "}
                                    {new Date(
                                        item.createdAt
                                    ).toLocaleString()}
                                </small>
                            </div>
                        </div>
                    ))}

                    {history.length === 0 && (
                        <p className="empty-state">
                            No workflow history found.
                        </p>
                    )}
                </div>
            </div>
        </div>
    );
}

export default WorkflowRequestDetailsPage;