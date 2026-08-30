import api from "./api";

export interface WorkflowComment {
    id: number;
    comment: string;
    userName: string;
    createdAt: string;
}

export const getWorkflowComments = async (
    workflowRequestId: number
): Promise<WorkflowComment[]> => {
    const response = await api.get<WorkflowComment[]>(
        `/workflowrequests/${workflowRequestId}/comments`
    );

    return response.data;
};

export const addWorkflowComment = async (
    workflowRequestId: number,
    comment: string
): Promise<WorkflowComment> => {
    const response = await api.post<WorkflowComment>(
        `/workflowrequests/${workflowRequestId}/comments`,
        {
            comment,
        }
    );

    return response.data;
};