export interface ComputerScopeAddRequest
{
    name : string;
    code : string;
    computer_type_id : number;
}

export interface ComputerScopeUpdateRequest extends ComputerScopeAddRequest
{
    id : number
    culture : string
}

