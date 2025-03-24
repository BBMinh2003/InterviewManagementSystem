import { Component, EventEmitter, Output } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { faDev } from '@fortawesome/free-brands-svg-icons';

@Component({
  selector: 'app-mobile-header',
  imports: [MatIconModule, FaIconComponent],
  templateUrl: './mobile-header.component.html',
  styleUrl: './mobile-header.component.css'
})
export class MobileHeaderComponent {
  faDev = faDev;
  @Output() mobileMenuToggle = new EventEmitter<void>();

  toggleMobileMenu() {
    this.mobileMenuToggle.emit();
  }
}
