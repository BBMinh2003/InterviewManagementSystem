import { HttpClient } from '@angular/common/http';
import { JobModel } from '../../models/job/job.model';
import { MasterDataService } from '../master-data/master-data.service';
import { IJobService } from './job-service.interface';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable()
export class JobService
  extends MasterDataService<JobModel>
  implements IJobService
{
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'job');
  }

  import(formData: FormData): Observable<any> {
    return this.httpClient.post(`${this.baseUrl+ '/import-excel'}`, formData);
  }

  override getById(id: string): Observable<JobModel> {
    return this.httpClient.get<JobModel>(`${this.baseUrl}/details/${id}`);
  }
}
