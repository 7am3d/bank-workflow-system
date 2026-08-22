import { useNavigate } from "react-router-dom";
import { useNotifications } from "../context/NotificationContext";

function Header() {
    const { unreadCount } = useNotifications();
    const navigate = useNavigate();

    return (
        <header className="header">
            <div>
                <h1>BankWorkflow</h1>
            </div>

            <div className="header-user">
                <button
                    className="notification-bell"
                    onClick={() => navigate("/notifications")}
                    aria-label="View notifications"
                >
                    🔔

                    {unreadCount > 0 && (
                        <span className="notification-badge">
                            {unreadCount}
                        </span>
                    )}
                </button>

                <span>Hamed Employee</span>
            </div>
        </header>
    );
}

export default Header;