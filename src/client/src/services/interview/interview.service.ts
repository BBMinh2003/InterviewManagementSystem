import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MasterDataService } from '../master-data/master-data.service';
import { InterviewModel } from '../../models/interview/interview.model';
import { IInterviewService } from './interview-service.interface';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InterviewService extends MasterDataService<InterviewModel> implements IInterviewService {
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'interview');
  }

  cancelInterview(interviewId: string): Observable<boolean> {
    return this.httpClient.put<boolean>(`${this.baseUrl}/cancel`, { interviewId });
  }

  sendReminder(interviewId: string): Observable<any> {
    return this.httpClient.post(`${this.baseUrl}/send-reminder`, { interviewId });
  }

  submitResult(id: string, result: number): Observable<boolean> {
    return this.httpClient.put<boolean>(`${this.baseUrl}/submit-result`, { id, result });
  }
}
