import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LevelModel } from '../../models/level/level.model';
import { MasterDataService } from '../master-data/master-data.service';
import { ILevelService } from './level-service.interface';

@Injectable({
  providedIn: 'root',
})
export class LevelService
  extends MasterDataService<LevelModel>
  implements ILevelService
{
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'Level');
  }
  getAllLevels() {
    return this.httpClient.get<{ id: string; name: string }[]>(this.baseUrl);
  }
}
