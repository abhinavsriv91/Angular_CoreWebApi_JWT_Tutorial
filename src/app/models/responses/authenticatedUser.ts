import { ApiResponse } from "./apiResponse";

export interface AuthenticatedUser extends ApiResponse
{
    userName : string;
    fullName : string;
    emailAddress : string;
    roles : string[]
    token? : string
}