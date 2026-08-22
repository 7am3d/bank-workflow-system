import { NavLink } from "react-router-dom";
import { useNotifications } from "../context/NotificationContext";

function Sidebar() {
    const { unreadCount } = useNotifications();

    return (
        <aside className="sidebar">
            <div className="sidebar-logo">
                <h2>BankWorkflow</h2>
            </div>

            <nav className="sidebar-nav">
                <NavLink
                    to="/dashboard"
                    className={({ isActive }) =>
                        isActive ? "nav-link active" : "nav-link"
                    }
                >
                    Dashboard
                </NavLink>

                <NavLink
                    to="/requests"
                    className={({ isActive }) =>
                        isActive ? "nav-link active" : "nav-link"
                    }
                >
                    Workflow Requests
                </NavLink>

                <NavLink
                    to="/notifications"
                    className={({ isActive }) =>
                        isActive ? "nav-link active" : "nav-link"
                    }
                >
                    <span>Notifications</span>

                    {unreadCount > 0 && (
                        <span className="sidebar-notification-badge">
                            {unreadCount}
                        </span>
                    )}
                </NavLink>
            </nav>

            <div className="sidebar-bottom">
                <NavLink to="/login" className="nav-link">
                    Logout
                </NavLink>
            </div>
        </aside>
    );
}

export default Sidebar;