import { ApiResponse } from './apiResponse';
import { Feature } from './featureResponse'

export interface FeatureOptionResponse extends ApiResponse{
    featureOptions : FeatureOption[]
}

export interface FeatureOption{
    id : number;
    name : string;
    feature : Feature
}