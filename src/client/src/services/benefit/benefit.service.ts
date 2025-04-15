import { Injectable } from '@angular/core';
import { BenefitModel } from '../../models/benefit/benefit.model';
import { HttpClient } from '@angular/common/http';
import { MasterDataService } from '../master-data/master-data.service';
import { IBenefitService } from './benefit-service.interface';

@Injectable({
  providedIn: 'root'
})
export class BenefitService extends MasterDataService<BenefitModel> implements IBenefitService {
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'Benefit');
   }
}
