import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { ILoadingService } from './loading-service.interface';

@Injectable({
  providedIn: 'root',
})
export class LoadingService implements ILoadingService {
  private loadingSubject = new BehaviorSubject<boolean>(false);
  loading$: Observable<boolean> = this.loadingSubject.asObservable()

  show() {
    this.loadingSubject.next(true);
  }

  hide() {   
    this.loadingSubject.next(false);
  }
}
