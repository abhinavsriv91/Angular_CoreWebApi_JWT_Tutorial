export interface BusinessADGroupAddRequest
{
    name : string;
    description : string;
}

export interface BusinessADGroupUpdateRequest extends BusinessADGroupAddRequest
{
    id : number;
    culture: string;
}

