import { SearchModel } from "../search.model";

export interface OfferSearch extends SearchModel{
    status?: number | null;
    departmentId?: string | null;
}