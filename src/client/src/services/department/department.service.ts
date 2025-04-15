import { Injectable } from '@angular/core';
import { MasterDataService } from '../master-data/master-data.service';
import { HttpClient } from '@angular/common/http';
import { DepartmentModel } from '../../models/department/department.model';
import { IDepartmentService } from './department-service.interface';

@Injectable({
  providedIn: 'root'
})
export class DepartmentService extends MasterDataService<DepartmentModel> implements IDepartmentService {
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'Department');
   }
   getAllDepartments() {
    return this.httpClient.get<{ id: string; name: string }[]>(this.baseUrl);
  }
}
