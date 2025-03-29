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
    console.log(' Loading started');
    this.loadingSubject.next(true);
  }

  hide() {   
     console.log('Loading stopped');
    this.loadingSubject.next(false);
  }
}
