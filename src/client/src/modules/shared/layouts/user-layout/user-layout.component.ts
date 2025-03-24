import { Component, HostListener } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../common/header/header.component';
import { SidebarComponent } from '../../common/sidebar/sidebar.component';
import { CommonModule } from '@angular/common';
import { MobileHeaderComponent } from '../../common/mobile-sidebar/mobile-header.component';

@Component({
  selector: 'app-user-layout',
  imports: [
    RouterOutlet,
    CommonModule,
    HeaderComponent,
    MobileHeaderComponent,
    SidebarComponent,
  ],
  templateUrl: './user-layout.component.html',
  styleUrl: './user-layout.component.css',
})
export class UserLayoutComponent {
  mobileMenuOpen = false;

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    if (window.innerWidth >= 768) {
      this.mobileMenuOpen = false;
    }
  }

  toggleMobileMenu() {
    this.mobileMenuOpen = !this.mobileMenuOpen;
  }

  onMobileOverlayClick() {
    this.mobileMenuOpen = false;
  }
}
