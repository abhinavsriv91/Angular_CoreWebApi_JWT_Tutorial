import { ApiResponse } from "./apiResponse";

export interface ComputerTypeResponse extends ApiResponse{
    computerTypes : ComputerType[]
}

export interface ComputerType{
    id : number;
    name : string;
}