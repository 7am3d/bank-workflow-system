import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import {
    getNotifications,
    markAllNotificationsAsRead,
    markNotificationAsRead,
} from "../services/notificationService";
import type { Notification } from "../services/notificationService";
import { useNotifications } from "../context/NotificationContext";

function NotificationsPage() {
    const navigate = useNavigate();

    const { refreshUnreadCount } = useNotifications();

    const [notifications, setNotifications] = useState<Notification[]>([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState("");
    const [markingAllRead, setMarkingAllRead] = useState(false);

    const loadNotifications = async () => {
        try {
            setError("");

            const data = await getNotifications();
            setNotifications(data);
        } catch (err) {
            console.error(err);
            setError("Failed to load notifications.");
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        loadNotifications();
    }, []);

    const handleMarkAsRead = async (notification: Notification) => {
        if (notification.isRead) {
            if (notification.workflowRequestId) {
                navigate(`/requests/${notification.workflowRequestId}`);
            }

            return;
        }

        try {
            await markNotificationAsRead(notification.id);

            setNotifications((current) =>
                current.map((item) =>
                    item.id === notification.id
                        ? { ...item, isRead: true }
                        : item
                )
            );

            // Immediately update the Header and Sidebar badge
            await refreshUnreadCount();

            if (notification.workflowRequestId) {
                navigate(`/requests/${notification.workflowRequestId}`);
            }
        } catch (err) {
            console.error(err);
            setError("Failed to mark notification as read.");
        }
    };

    const handleMarkAllAsRead = async () => {
        try {
            setMarkingAllRead(true);
            setError("");

            await markAllNotificationsAsRead();

            setNotifications((current) =>
                current.map((notification) => ({
                    ...notification,
                    isRead: true,
                }))
            );

            // Immediately update the Header and Sidebar badge
            await refreshUnreadCount();
        } catch (err) {
            console.error(err);
            setError("Failed to mark notifications as read.");
        } finally {
            setMarkingAllRead(false);
        }
    };

    const unreadCount = notifications.filter(
        (notification) => !notification.isRead
    ).length;

    if (loading) {
        return (
            <div className="dashboard-page">
                <div className="page-header">
                    <h1>Notifications</h1>
                    <p>Loading your notifications...</p>
                </div>
            </div>
        );
    }

    return (
        <div className="dashboard-page">
            <div className="page-header">
                <h1>Notifications</h1>
                <p>Stay up to date with your workflow activity.</p>
            </div>

            {error && (
                <p className="error-message">
                    {error}
                </p>
            )}

            <div className="dashboard-section">
                <div className="section-header">
                    <div>
                        <h2>
                            Notifications{" "}
                            {unreadCount > 0 && (
                                <span className="notification-count">
                                    {unreadCount} unread
                                </span>
                            )}
                        </h2>

                        <p>
                            Requests, approvals, and workflow updates.
                        </p>
                    </div>

                    {unreadCount > 0 && (
                        <div className="notification-actions">
                            <button
                                className="mark-read-button"
                                onClick={handleMarkAllAsRead}
                                disabled={markingAllRead}
                            >
                                {markingAllRead
                                    ? "Marking..."
                                    : "Mark all as read"}
                            </button>
                        </div>
                    )}
                </div>

                <div className="notification-list">
                    {notifications.map((notification) => (
                        <div
                            key={notification.id}
                            className={`notification-item ${notification.isRead
                                    ? "read"
                                    : "unread"
                                }`}
                            onClick={() =>
                                handleMarkAsRead(notification)
                            }
                        >
                            <div className="notification-content">
                                <div className="notification-header">
                                    <h3>
                                        {notification.title}
                                    </h3>

                                    {!notification.isRead && (
                                        <span className="unread-dot" />
                                    )}
                                </div>

                                <p>{notification.message}</p>

                                <small>
                                    {new Date(
                                        notification.createdAt
                                    ).toLocaleString()}
                                </small>
                            </div>
                        </div>
                    ))}

                    {notifications.length === 0 && (
                        <p className="empty-state">
                            You have no notifications.
                        </p>
                    )}
                </div>
            </div>
        </div>
    );
}

export default NotificationsPage;