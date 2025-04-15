import { CommonModule } from '@angular/common';
import { Component, HostListener } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HeaderComponent } from '../../common/header/header.component';
import { MobileHeaderComponent } from '../../common/mobile-sidebar/mobile-header.component';
import { SidebarComponent } from '../../common/sidebar/sidebar.component';

@Component({
  selector: 'app-admin-layout',
  imports: [
    RouterOutlet,
    CommonModule,
    HeaderComponent,
    MobileHeaderComponent,
    SidebarComponent,
  ],
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.css',
})
export class AdminLayoutComponent {
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
