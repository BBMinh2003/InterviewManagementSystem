import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { MasterDataService } from '../master-data/master-data.service';
import { ContactTypeModel } from '../../models/contactType/contactType.model';
import { HttpClient } from '@angular/common/http';
import { IDashboardService } from './dashboard-service.interface';
import DashboardView from '../../models/common/dashboard.model';

@Injectable({
  providedIn: 'root',
})
export class DashboardService implements IDashboardService {
  protected readonly baseUrl = 'http://localhost:5108/api/';

  constructor(protected httpClient: HttpClient) {
  }

  getData(): Observable<DashboardView> {
    return this.httpClient.get<DashboardView>(this.baseUrl + "dashboard");
  }

}
