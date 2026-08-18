import { useState } from "react";
import { useNavigate } from "react-router-dom";
import api from "../services/api";

function CreateWorkflowRequestPage() {
    const navigate = useNavigate();

    const [title, setTitle] = useState("");
    const [description, setDescription] = useState("");
    const [requestTypeId, setRequestTypeId] = useState(1);
    const [priority, setPriority] = useState("Medium");

    const [loading, setLoading] = useState(false);
    const [error, setError] = useState("");

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();

        setError("");

        if (!title.trim()) {
            setError("Please enter a title.");
            return;
        }

        if (!description.trim()) {
            setError("Please enter a description.");
            return;
        }

        try {
            setLoading(true);

            const response = await api.post("/WorkflowRequests", {
                requestTypeId,
                title: title.trim(),
                description: description.trim(),
                priority,
            });

            // Open the newly created request
            navigate(`/requests/${response.data.id}`);
        } catch (err) {
            console.error(err);
            setError("Failed to create workflow request.");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="dashboard-page">
            <div className="page-header">
                <button onClick={() => navigate("/requests")}>
                    ← Back to Requests
                </button>

                <h1>New Workflow Request</h1>
                <p>Create a new request for approval.</p>
            </div>

            <div className="dashboard-section">
                <form onSubmit={handleSubmit}>

                    {error && (
                        <div className="error-message">
                            {error}
                        </div>
                    )}

                    <div className="form-group">
                        <label htmlFor="title">
                            Title
                        </label>

                        <input
                            id="title"
                            type="text"
                            value={title}
                            onChange={(e) => setTitle(e.target.value)}
                            placeholder="Enter request title"
                        />
                    </div>

                    <div className="form-group">
                        <label htmlFor="requestType">
                            Request Type
                        </label>

                        <select
                            id="requestType"
                            value={requestTypeId}
                            onChange={(e) =>
                                setRequestTypeId(Number(e.target.value))
                            }
                        >
                            <option value={1}>
                                Leave Request
                            </option>
                        </select>
                    </div>

                    <div className="form-group">
                        <label htmlFor="priority">
                            Priority
                        </label>

                        <select
                            id="priority"
                            value={priority}
                            onChange={(e) => setPriority(e.target.value)}
                        >
                            <option value="Low">Low</option>
                            <option value="Medium">Medium</option>
                            <option value="High">High</option>
                        </select>
                    </div>

                    <div className="form-group">
                        <label htmlFor="description">
                            Description
                        </label>

                        <textarea
                            id="description"
                            value={description}
                            onChange={(e) =>
                                setDescription(e.target.value)
                            }
                            placeholder="Describe your request"
                            rows={6}
                        />
                    </div>

                    <div className="form-actions">
                        <button
                            type="button"
                            onClick={() => navigate("/requests")}
                            disabled={loading}
                        >
                            Cancel
                        </button>

                        <button
                            type="submit"
                            disabled={loading}
                        >
                            {loading
                                ? "Submitting..."
                                : "Submit Request"}
                        </button>
                    </div>
                </form>
            </div>
        </div>
    );
}

export default CreateWorkflowRequestPage;