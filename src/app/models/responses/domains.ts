import { ApiResponse } from "./apiResponse";

export interface Domains extends ApiResponse
{
    domains : Domain[]
} 

export interface Domain
{
    id_domain : number;
    description : string;
    fqdn : string;
    is_default : boolean
}