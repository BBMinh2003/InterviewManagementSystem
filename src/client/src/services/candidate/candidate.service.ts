import { Injectable } from '@angular/core';
import { CandidateModel } from '../../models/candidate/candidate.model';
import { HttpClient } from '@angular/common/http';
import { MasterDataService } from '../master-data/master-data.service';
import { ICandidateService } from './candidate-service.interface';

@Injectable({
  providedIn: 'root',
})
export class CandidateService extends MasterDataService<CandidateModel> implements ICandidateService {
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'candidates');
  }
}
