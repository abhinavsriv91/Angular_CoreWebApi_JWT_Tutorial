import { ApiResponse } from "./apiResponse";

export interface ComputerScopeResponse extends ApiResponse{
    computerScopes : ComputerScope[]
}

export interface ComputerScope{
    id : number;
    name : string;
    code: string;
    filter: string;
    computer_type_id: number;
    computer_type_name: string;
}