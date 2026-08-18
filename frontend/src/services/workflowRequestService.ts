import api from "./api";

export interface PendingApprovalRequest {
    id: number;
    title: string;
    requestedBy: string;
    priority: string;
    createdAt: string;
}

export const getPendingApprovals = async (): Promise<
    PendingApprovalRequest[]
> => {
    const response = await api.get<PendingApprovalRequest[]>(
        "/WorkflowRequests/pending-approvals"
    );

    return response.data;
};