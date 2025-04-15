import { Injectable } from '@angular/core';
import { MasterDataService } from '../master-data/master-data.service';
import { IOfferService } from './offer-service.interface';
import { HttpClient, HttpParams } from '@angular/common/http';
import { OfferModel } from '../../models/offer/offer.model';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class OfferService
  extends MasterDataService<OfferModel>
  implements IOfferService
{
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'offer');
  }
  updateOfferStatus(id: string, status: number): Observable<any> {
    return this.httpClient.put(
      `${this.baseUrl}/update-status`,
      { offerId: id, offerStatus: status },
      { responseType: 'text' }
    );
  }

  createOffer(data: any): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/create`, data, {
      headers: { 'Content-Type': 'application/json' },
    });
  }

  exportOffers(fromDate: Date, toDate: Date): Observable<Blob> {
    const params = new HttpParams()
      .set('fromDate', fromDate.toISOString())
      .set('toDate', toDate.toISOString());

    return this.httpClient.get(`${this.baseUrl}/export`, {
      params,
      responseType: 'blob' 
    });
  }
}
