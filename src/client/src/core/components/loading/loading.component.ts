import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ILoadingService } from '../../../services/loading/loading-service.interface';
import { LOADING_SERVICE } from '../../../constants/injection.constant';
@Component({
  selector: 'app-loading',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './loading.component.html',
  styleUrls: ['./loading.component.scss'],
})
export class LoadingComponent {
  constructor(
    @Inject(LOADING_SERVICE)
    public loadingService: ILoadingService
  ) {}
  
}
