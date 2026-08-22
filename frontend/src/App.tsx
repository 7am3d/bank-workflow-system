import { Navigate, Route, Routes } from "react-router-dom";
import { NotificationProvider } from "./context/NotificationContext";

import MainLayout from "./layouts/MainLayout";
import DashboardPage from "./pages/DashboardPage";
import Login from "./pages/Login";
import WorkflowRequestsPage from "./pages/WorkflowRequestsPage";
import WorkflowRequestDetailsPage from "./pages/WorkflowRequestDetailsPage";
import CreateWorkflowRequestPage from "./pages/CreateWorkflowRequestPage";
import PendingApprovalsPage from "./pages/PendingApprovalsPage";
import NotificationsPage from "./pages/NotificationsPage";

function App() {
    return (
        <NotificationProvider>
            <Routes>
                <Route path="/login" element={<Login />} />

                <Route element={<MainLayout />}>
                    <Route
                        path="/"
                        element={<Navigate to="/dashboard" replace />}
                    />

                    <Route
                        path="/dashboard"
                        element={<DashboardPage />}
                    />

                    <Route
                        path="/requests"
                        element={<WorkflowRequestsPage />}
                    />

                    <Route
                        path="/requests/:id"
                        element={<WorkflowRequestDetailsPage />}
                    />

                    <Route
                        path="/requests/new"
                        element={<CreateWorkflowRequestPage />}
                    />

                    <Route
                        path="/pending-approvals"
                        element={<PendingApprovalsPage />}
                    />

                    <Route
                        path="/notifications"
                        element={<NotificationsPage />}
                    />
                </Route>

                <Route
                    path="*"
                    element={<Navigate to="/dashboard" replace />}
                />
            </Routes>
        </NotificationProvider>
    );
}

export default App;