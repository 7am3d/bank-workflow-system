import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import api from "../services/api";
import {
    getWorkflowComments,
    addWorkflowComment,
} from "../services/commentService";

import type { WorkflowComment } from "../services/commentService";

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

    const [comments, setComments] = useState<WorkflowComment[]>([]);
    const [commentText, setCommentText] = useState("");
    const [commentsLoading, setCommentsLoading] = useState(true);
    const [commentLoading, setCommentLoading] = useState(false);
    const [commentError, setCommentError] = useState("");

    useEffect(() => {
        const loadRequest = async () => {
            try {
                setLoading(true);
                setError("");

                const response = await api.get(`/WorkflowRequests/${id}`);
                setRequest(response.data);

                const historyResponse = await api.get(
                    `/WorkflowRequests/${id}/history`
                );
                setHistory(historyResponse.data);

                const commentsResponse = await getWorkflowComments(Number(id));
                setComments(commentsResponse);
            } catch (err) {
                console.error(err);
                setError("Failed to load workflow request.");
            } finally {
                setLoading(false);
                setCommentsLoading(false);
            }
        };

        loadRequest();
    }, [id]);

    const refreshRequestData = async () => {
        const response = await api.get(`/WorkflowRequests/${id}`);
        setRequest(response.data);

        const historyResponse = await api.get(
            `/WorkflowRequests/${id}/history`
        );
        setHistory(historyResponse.data);
    };

    const handleApprove = async () => {
        try {
            setActionLoading(true);
            setActionError("");

            await api.post(`/WorkflowRequests/${id}/approve`);
            await refreshRequestData();
        } catch (err) {
            console.error(err);
            setActionError("Failed to approve workflow request.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleReject = async () => {
        const reason = window.prompt(
            "Please enter a reason for rejecting this request:"
        );

        if (!reason || !reason.trim()) {
            return;
        }

        try {
            setActionLoading(true);
            setActionError("");

            await api.post(`/WorkflowRequests/${id}/reject`, {
                reason: reason.trim(),
            });

            await refreshRequestData();
        } catch (err) {
            console.error(err);
            setActionError("Failed to reject workflow request.");
        } finally {
            setActionLoading(false);
        }
    };

    const handleAddComment = async () => {
        const trimmedComment = commentText.trim();

        if (!trimmedComment) {
            return;
        }

        if (trimmedComment.length > 2000) {
            setCommentError("Comment cannot exceed 2000 characters.");
            return;
        }

        try {
            setCommentLoading(true);
            setCommentError("");

            const newComment = await addWorkflowComment(
                Number(id),
                trimmedComment
            );

            setComments((currentComments) => [
                ...currentComments,
                newComment,
            ]);

            setCommentText("");
        } catch (err) {
            console.error(err);
            setCommentError("Failed to add comment. Please try again.");
        } finally {
            setCommentLoading(false);
        }
    };

    const getInitials = (name: string) => {
        return name
            .split(" ")
            .filter(Boolean)
            .map((part) => part[0])
            .join("")
            .substring(0, 2)
            .toUpperCase();
    };

    const getStatusClass = (status: string) => {
        switch (status.toLowerCase()) {
            case "approved":
                return "status-approved";

            case "rejected":
                return "status-rejected";

            case "pending":
                return "status-pending";

            default:
                return "status-default";
        }
    };

    const getPriorityClass = (priority: string) => {
        switch (priority.toLowerCase()) {
            case "high":
                return "priority-high";

            case "medium":
                return "priority-medium";

            case "low":
                return "priority-low";

            default:
                return "priority-default";
        }
    };

    if (loading) {
        return (
            <div className="details-page">
                <div className="loading-card">
                    <div className="loading-spinner" />
                    <h2>Loading request</h2>
                    <p>Please wait while the workflow details are loaded.</p>
                </div>
            </div>
        );
    }

    if (error) {
        return (
            <div className="details-page">
                <div className="error-card">
                    <div className="error-icon">!</div>
                    <h2>Unable to load request</h2>
                    <p>{error}</p>

                    <button
                        className="secondary-button"
                        onClick={() => navigate("/requests")}
                    >
                        ← Back to Requests
                    </button>
                </div>
            </div>
        );
    }

    if (!request) {
        return (
            <div className="details-page">
                <div className="error-card">
                    <div className="error-icon">!</div>
                    <h2>Request not found</h2>
                    <p>
                        The workflow request you're looking for could not be
                        found.
                    </p>

                    <button
                        className="secondary-button"
                        onClick={() => navigate("/requests")}
                    >
                        ← Back to Requests
                    </button>
                </div>
            </div>
        );
    }

    return (
        <div className="details-page">

            {/* =========================
                PAGE HEADER
            ========================= */}

            <div className="details-page-header">

                <button
                    className="back-button"
                    onClick={() => navigate("/requests")}
                >
                    <span>←</span>
                    Back to Requests
                </button>

                <div className="details-heading-row">

                    <div>
                        <div className="request-reference">
                            REQUEST #{request.id}
                        </div>

                        <h1>{request.title}</h1>

                        <p className="details-subtitle">
                            Review and manage this workflow request
                        </p>
                    </div>

                    <div className="header-statuses">
                        <span
                            className={`status-badge-large ${getStatusClass(
                                request.status
                            )}`}
                        >
                            <span className="status-dot" />
                            {request.status}
                        </span>

                        <span
                            className={`priority-badge-large ${getPriorityClass(
                                request.priority
                            )}`}
                        >
                            {request.priority} priority
                        </span>
                    </div>

                </div>
            </div>


            {/* =========================
                REQUEST SUMMARY
            ========================= */}

            <div className="request-summary-card">

                <div className="section-header">
                    <div>
                        <h2>Request Details</h2>
                        <p>
                            Information associated with this workflow request.
                        </p>
                    </div>
                </div>

                <div className="request-summary-body">

                    <div className="request-description">
                        <span className="detail-label">Description</span>

                        <p>{request.description}</p>
                    </div>

                    <div className="detail-grid">

                        <div className="detail-item">
                            <span className="detail-label">
                                Request Type
                            </span>

                            <strong>{request.requestType}</strong>
                        </div>

                        <div className="detail-item">
                            <span className="detail-label">
                                Created By
                            </span>

                            <strong>{request.createdBy}</strong>
                        </div>

                        <div className="detail-item">
                            <span className="detail-label">
                                Current Step
                            </span>

                            <strong>Step {request.currentStep}</strong>
                        </div>

                        <div className="detail-item">
                            <span className="detail-label">
                                Created
                            </span>

                            <strong>
                                {new Date(
                                    request.createdAt
                                ).toLocaleDateString(undefined, {
                                    day: "2-digit",
                                    month: "short",
                                    year: "numeric",
                                })}
                            </strong>
                        </div>

                    </div>
                </div>
            </div>


            {/* =========================
                WORKFLOW ACTIONS
            ========================= */}

            {request.status === "Pending" &&
                request.canCurrentUserAct && (
                    <div className="action-card">

                        <div className="action-card-content">

                            <div className="action-icon">
                                ✓
                            </div>

                            <div>
                                <h2>Action Required</h2>

                                <p>
                                    This request is waiting for your review.
                                    Please approve or reject it to continue the
                                    workflow.
                                </p>
                            </div>

                        </div>

                        {actionError && (
                            <div className="action-error">
                                <span>!</span>
                                {actionError}
                            </div>
                        )}

                        <div className="workflow-actions">

                            <button
                                className="approve-button"
                                onClick={handleApprove}
                                disabled={actionLoading}
                            >
                                <span className="button-icon">✓</span>

                                {actionLoading
                                    ? "Processing..."
                                    : "Approve Request"}
                            </button>

                            <button
                                className="reject-button"
                                onClick={handleReject}
                                disabled={actionLoading}
                            >
                                <span className="button-icon">×</span>

                                {actionLoading
                                    ? "Processing..."
                                    : "Reject Request"}
                            </button>

                        </div>
                    </div>
                )}


            {/* =========================
                WORKFLOW HISTORY
            ========================= */}

            <div className="dashboard-section">

                <div className="section-header">
                    <div>
                        <h2>Workflow History</h2>

                        <p>
                            Complete activity and status changes for this
                            request.
                        </p>
                    </div>

                    <span className="activity-count">
                        {history.length}{" "}
                        {history.length === 1
                            ? "activity"
                            : "activities"}
                    </span>
                </div>

                <div className="history-list">

                    {history.length === 0 ? (
                        <div className="history-empty">
                            <div className="history-empty-icon">
                                ◷
                            </div>

                            <h3>No workflow history</h3>

                            <p>
                                No activity has been recorded for this request
                                yet.
                            </p>
                        </div>
                    ) : (
                        history.map((item, index) => {

                            const actionLower =
                                item.action.toLowerCase();

                            const isApproved =
                                actionLower.includes("approved");

                            const isRejected =
                                actionLower.includes("rejected");

                            const dotClass = isApproved
                                ? "history-dot-approved"
                                : isRejected
                                    ? "history-dot-rejected"
                                    : "history-dot-default";

                            return (
                                <div
                                    className="history-item"
                                    key={item.id}
                                >

                                    <div className="history-timeline">

                                        <div
                                            className={`history-dot ${dotClass}`}
                                        >
                                            {isApproved
                                                ? "✓"
                                                : isRejected
                                                    ? "×"
                                                    : ""}
                                        </div>

                                        {index !==
                                            history.length - 1 && (
                                                <div className="history-line" />
                                            )}

                                    </div>

                                    <div className="history-content">

                                        <div className="history-top">

                                            <div>
                                                <h3>
                                                    {item.action}
                                                </h3>
                                            </div>

                                            <time>
                                                {new Date(
                                                    item.createdAt
                                                ).toLocaleString(
                                                    undefined,
                                                    {
                                                        day: "2-digit",
                                                        month: "short",
                                                        year: "numeric",
                                                        hour: "2-digit",
                                                        minute: "2-digit",
                                                    }
                                                )}
                                            </time>

                                        </div>

                                        {item.previousStatus !== null &&
                                            item.previousStatus !==
                                            item.newStatus && (
                                                <div className="history-status">

                                                    <span className="history-status-old">
                                                        {item.previousStatus}
                                                    </span>

                                                    <span className="history-arrow">
                                                        →
                                                    </span>

                                                    <span className="history-status-new">
                                                        {item.newStatus}
                                                    </span>

                                                </div>
                                            )}

                                        {item.previousStatus === null &&
                                            item.newStatus && (
                                                <div className="history-status">

                                                    <span>
                                                        Status
                                                    </span>

                                                    <span className="history-arrow">
                                                        →
                                                    </span>

                                                    <span className="history-status-new">
                                                        {item.newStatus}
                                                    </span>

                                                </div>
                                            )}

                                        {item.details && (
                                            <p className="history-details">
                                                {item.details}
                                            </p>
                                        )}

                                        <div className="history-user">
                                            Performed by{" "}
                                            <strong>
                                                {item.performedBy}
                                            </strong>
                                        </div>

                                    </div>
                                </div>
                            );
                        })
                    )}

                </div>
            </div>


            {/* =========================
                COMMENTS
            ========================= */}

            <div className="dashboard-section comments-section">

                <div className="section-header">

                    <div>
                        <h2>Comments</h2>

                        <p>
                            Discussion and notes related to this workflow
                            request.
                        </p>
                    </div>

                    <span className="comment-count">
                        {comments.length}
                        {" "}
                        {comments.length === 1
                            ? "comment"
                            : "comments"}
                    </span>

                </div>


                <div className="comments-body">

                    {commentError && (
                        <div className="comment-error">
                            <span>!</span>
                            {commentError}
                        </div>
                    )}


                    {commentsLoading ? (
                        <div className="comments-loading">
                            <div className="loading-spinner small" />
                            Loading comments...
                        </div>
                    ) : (
                        <>

                            <div className="comments-list">

                                {comments.length === 0 ? (
                                    <div className="comments-empty">

                                        <div className="comments-empty-icon">
                                            💬
                                        </div>

                                        <h3>
                                            No comments yet
                                        </h3>

                                        <p>
                                            Start a discussion by adding the
                                            first comment to this workflow
                                            request.
                                        </p>

                                    </div>
                                ) : (
                                    comments.map((comment) => (
                                        <div
                                            className="comment-item"
                                            key={comment.id}
                                        >

                                            <div className="comment-avatar">
                                                {getInitials(
                                                    comment.userName
                                                )}
                                            </div>

                                            <div className="comment-content">

                                                <div className="comment-header">

                                                    <strong>
                                                        {
                                                            comment.userName
                                                        }
                                                    </strong>

                                                    <time className="comment-date">
                                                        {new Date(
                                                            comment.createdAt
                                                        ).toLocaleString(
                                                            undefined,
                                                            {
                                                                day: "2-digit",
                                                                month: "short",
                                                                year: "numeric",
                                                                hour: "2-digit",
                                                                minute: "2-digit",
                                                            }
                                                        )}
                                                    </time>

                                                </div>

                                                <p className="comment-text">
                                                    {comment.comment}
                                                </p>

                                            </div>

                                        </div>
                                    ))
                                )}

                            </div>


                            <div className="comment-form">

                                <div className="comment-form-label">

                                    <label htmlFor="comment">
                                        Add a comment
                                    </label>

                                    <span className="character-count">
                                        {commentText.length}/2000
                                    </span>

                                </div>

                                <textarea
                                    id="comment"
                                    value={commentText}
                                    onChange={(e) =>
                                        setCommentText(
                                            e.target.value
                                        )
                                    }
                                    placeholder="Write a comment about this workflow request..."
                                    maxLength={2000}
                                    disabled={commentLoading}
                                />

                                <div className="comment-form-footer">

                                    <span className="comment-hint">
                                        Comments are visible to employees
                                        involved in this workflow.
                                    </span>

                                    <button
                                        className="add-comment-button"
                                        onClick={handleAddComment}
                                        disabled={
                                            commentLoading ||
                                            !commentText.trim()
                                        }
                                    >
                                        {commentLoading
                                            ? "Adding..."
                                            : "Add Comment"}
                                    </button>

                                </div>

                            </div>

                        </>
                    )}

                </div>
            </div>

        </div>
    );
}

export default WorkflowRequestDetailsPage;