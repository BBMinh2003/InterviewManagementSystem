import { Observable } from "rxjs";
import { OfferModel } from "../../models/offer/offer.model";
import { IMasterDataService } from "../master-data/master-data-service.interface";

export interface IOfferService extends IMasterDataService<OfferModel> {
    updateOfferStatus(id: string, status: number): Observable<any>
    createOffer(formData: FormData): Observable<any>
    exportOffers(fromDate: Date, toDate: Date): Observable<Blob>
}