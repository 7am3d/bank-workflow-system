import api from "./api";

export interface DashboardData {
    myRequests: number;
    myPendingRequests: number;
    myApprovedRequests: number;
    myRejectedRequests: number;
    pendingApprovals: number;
    totalRequests: number;
    totalPending: number;
    totalApproved: number;
    totalRejected: number;
    recentRequests: RecentRequest[];
    pendingApprovalRequests: PendingApprovalRequest[];
}

export interface RecentRequest {
    id: number;
    title: string;
    status: string;
    priority: string;
    createdAt: string;
}

export interface PendingApprovalRequest {
    id: number;
    title: string;
    requestedBy: string;
    priority: string;
    createdAt: string;
}

export const getDashboard = async (): Promise<DashboardData> => {
    const response = await api.get<DashboardData>("/Dashboard");

    return response.data;
};