import { ApiResponse } from "./apiResponse";

export interface BusinessADGroupResponse extends ApiResponse{
    businessADGroups : BusinessADGroup[]
}

export interface BusinessADGroup{
    id : number;
    name : string;
    description : string;
}