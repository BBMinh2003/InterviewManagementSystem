import { Injectable } from '@angular/core';
import { SkillModel } from '../../models/skill/skill.model';
import { HttpClient } from '@angular/common/http';
import { MasterDataService } from '../master-data/master-data.service';
import { ISkillService } from './skill-service.interface';

@Injectable({
  providedIn: 'root'
})
export class SkillService extends MasterDataService<SkillModel> implements ISkillService {
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'Skill');
   }
}
