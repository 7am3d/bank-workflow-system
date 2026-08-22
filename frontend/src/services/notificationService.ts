import api from "./api";

export interface Notification {
    id: number;
    title: string;
    message: string;
    type: string;
    isRead: boolean;
    createdAt: string;
    workflowRequestId: number | null;
}

export const getNotifications = async (): Promise<Notification[]> => {
    const response = await api.get<Notification[]>(
        "/Notifications"
    );

    return response.data;
};

export const getUnreadNotificationCount = async (): Promise<number> => {
    const response = await api.get<number>(
        "/Notifications/unread-count"
    );

    return response.data;
};

export const markNotificationAsRead = async (
    notificationId: number
): Promise<void> => {
    await api.put(
        `/Notifications/${notificationId}/read`
    );
};

export const markAllNotificationsAsRead = async (): Promise<void> => {
    await api.put(
        "/Notifications/read-all"
    );
};