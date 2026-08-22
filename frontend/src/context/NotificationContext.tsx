import {
    createContext,
    useContext,
    useEffect,
    useState,
    type ReactNode,
} from "react";

import { getUnreadNotificationCount } from "../services/notificationService";

interface NotificationContextType {
    unreadCount: number;
    refreshUnreadCount: () => Promise<void>;
}

const NotificationContext =
    createContext<NotificationContextType | undefined>(undefined);

export function NotificationProvider({
    children,
}: {
    children: ReactNode;
}) {
    const [unreadCount, setUnreadCount] = useState(0);

    const refreshUnreadCount = async () => {
        try {
            const count = await getUnreadNotificationCount();
            setUnreadCount(count);
        } catch (error) {
            console.error("Failed to load unread notification count.", error);
        }
    };

    useEffect(() => {
        refreshUnreadCount();

        const interval = setInterval(() => {
            refreshUnreadCount();
        }, 5000);

        return () => clearInterval(interval);
    }, []);

    return (
        <NotificationContext.Provider
            value={{
                unreadCount,
                refreshUnreadCount,
            }}
        >
            {children}
        </NotificationContext.Provider>
    );
}

export function useNotifications() {
    const context = useContext(NotificationContext);

    if (!context) {
        throw new Error(
            "useNotifications must be used inside NotificationProvider"
        );
    }

    return context;
}