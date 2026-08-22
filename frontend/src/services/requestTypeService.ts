import api from "./api";

export interface RequestType {
    id: number;
    name: string;
    description: string;
    isActive: boolean;
    createdAt: string;
}

export const getRequestTypes = async (): Promise<RequestType[]> => {
    const response = await api.get<RequestType[]>("/RequestTypes");

    return response.data.filter((requestType) => requestType.isActive);
};