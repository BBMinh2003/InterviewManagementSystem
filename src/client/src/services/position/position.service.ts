import { Injectable } from '@angular/core';
import { MasterDataService } from '../master-data/master-data.service';
import { PositionModel } from '../../models/position/position.model';
import { HttpClient } from '@angular/common/http';
import { IPositionService } from './position-service.interface';

@Injectable({
  providedIn: 'root'
})
export class PositionService extends MasterDataService<PositionModel> implements IPositionService {
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'Position');
   }
}
