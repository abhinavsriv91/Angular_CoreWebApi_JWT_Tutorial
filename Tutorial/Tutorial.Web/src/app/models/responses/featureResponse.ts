import { ApiResponse } from "./apiResponse";

export interface FeatureResponse extends ApiResponse{
    features : Feature[]
}

export interface Feature{
    id : number;
    name : string;
}