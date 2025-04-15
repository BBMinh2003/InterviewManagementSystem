import { Injectable } from '@angular/core';
import { CandidateModel } from '../../models/candidate/candidate.model';
import { HttpClient } from '@angular/common/http';
import { MasterDataService } from '../master-data/master-data.service';
import { ICandidateService } from './candidate-service.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class CandidateService extends MasterDataService<CandidateModel> implements ICandidateService {
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'Candidate');
  }

  updateCandidate(id: string, formData: FormData): Observable<any> {
    return this.httpClient.put(`${this.baseUrl+ '/update'}/${id}`, formData);
  }

  createCandidate(formData: FormData): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/create`, formData);
  }

  banCandidate(id: string): Observable<boolean> {
    return this.httpClient.put<boolean>(`${this.baseUrl+ '/ban'}/${id}`, id);
  }
}
