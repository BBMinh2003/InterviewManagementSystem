import { Injectable } from '@angular/core';
import { InterviewerModel } from '../../models/user/interview.model';
import { MasterDataService } from '../master-data/master-data.service';
import { HttpClient } from '@angular/common/http';
import { IInterviewerService } from './interviewer-service.interface';
import { Observable } from 'rxjs';

@Injectable()
export class InterviewerService
  extends MasterDataService<InterviewerModel>
  implements IInterviewerService
{
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'GetUser');
  }
  getAdmins(): Observable<InterviewerModel[]> {
    return this.httpClient.get<InterviewerModel[]>(`${this.baseUrl}/admin`);
  }
  getInterviewers(): Observable<InterviewerModel[]> {
    return this.httpClient.get<InterviewerModel[]>(`${this.baseUrl}/interviewers`);
  }

  getRecruitersAndManagers(): Observable<InterviewerModel[]> {
    return this.httpClient.get<InterviewerModel[]>(
      `${this.baseUrl}/recruiters`
    );
  }
}
