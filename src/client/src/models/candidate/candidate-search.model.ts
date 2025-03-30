import { SearchModel } from "../search.model";

export interface CandidateSearch extends SearchModel {
    status?: number;
}