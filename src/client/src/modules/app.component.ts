import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NotificationComponent } from "../core/components/notification/notification.component";
import { LoadingComponent } from '../core/components/loading/loading.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NotificationComponent, LoadingComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
})
export class AppComponent {
  title = 'interview-management-system';
}
