import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NotificationComponent } from "../core/components/notification/notification.component";

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, NotificationComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'interview-management-system';
}
