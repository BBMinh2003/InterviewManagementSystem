import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { PaginatedResult } from '../../models/paginated-result.model';
import { IMasterDataService } from './master-data-service.interface';

@Injectable({
  providedIn: 'root',
})
export class MasterDataService<T> implements IMasterDataService<T> {
  protected readonly baseUrl = 'http://localhost:5108/api/';

  constructor(protected httpClient: HttpClient, protected apiUrl: String) {
    this.baseUrl += apiUrl;
  }

  getAll(): Observable<T[]> {
    return this.httpClient.get<T[]>(this.baseUrl);
  }

  search(filter: any): Observable<PaginatedResult<T>> {
    return this.httpClient.post<PaginatedResult<T>>(
      this.baseUrl + '/search',
      filter
    );
  }

  getById(id: string): Observable<T> {
    return this.httpClient.get<T>(`${this.baseUrl}/${id}`);
  }

  create(data: T): Observable<T> {
    return this.httpClient.post<T>(`${this.baseUrl}/create`, data);
  }

  update(id: string, data: T): Observable<T> {
    return this.httpClient.put<T>(`${this.baseUrl+ '/update'}/${id}`, data);
  }

  delete(id: string): Observable<boolean> {
    return this.httpClient.delete<boolean>(`${this.baseUrl}/${id}`);
  }
}
