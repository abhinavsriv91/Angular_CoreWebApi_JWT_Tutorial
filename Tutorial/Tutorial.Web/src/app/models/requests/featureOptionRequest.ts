export interface FeatureOptionAddRequest
{
    name : string;
    feature_id : number;
}

export interface FeatureOptionUpdateRequest extends FeatureOptionAddRequest
{
    id : number;
    culture: string;
}

