import { Component, ElementRef, HostListener } from '@angular/core';
import {
  faHome,
  faUsersLine,
  faUserCircle,
  faBriefcase,
  faComment,
  faFileLines,
  faUser,
} from '@fortawesome/free-solid-svg-icons';
import { faDev } from '@fortawesome/free-brands-svg-icons';
import { FaIconComponent } from '@fortawesome/angular-fontawesome';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, FaIconComponent, MatIconModule, RouterModule],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  open = true;
  faUserCircle = faUserCircle;
  faDev = faDev;
  menuItems = [
    { icon: faHome, label: 'Home', route: '/home' },
    { icon: faUsersLine, label: 'Candidate', route: '/candidate' },
    { icon: faBriefcase, label: 'Job', route: '/job' },
    { icon: faComment, label: 'Interview', route: '/interview' },
    { icon: faFileLines, label: 'Offer', route: '/offer' },
    { icon: faUser, label: 'User', route: '/user' },
  ];

  toggleSidebar() {
    this.open = !this.open;
  }
  isUserMenuOpen = false;

  toggleUserMenu() {
    this.isUserMenuOpen = !this.isUserMenuOpen;
  }

  handleProfile() {
    this.isUserMenuOpen = false;
    console.log('Navigate to profile');
  }

  handleLogout() {
    this.isUserMenuOpen = false;
    console.log('Logout logic');
  }

  closeUserMenu() {
    this.isUserMenuOpen = false;
  }

  @HostListener('document:click', ['$event'])
  onDocumentClick(event: MouseEvent) {
    const target = event.target as HTMLElement;
    const menu = this.elementRef.nativeElement.querySelector('.user-menu');
    const trigger =
      this.elementRef.nativeElement.querySelector('.user-menu-trigger');

    if (!menu?.contains(target) && !trigger?.contains(target)) {
      this.closeUserMenu();
    }
  }

  @HostListener('document:keydown', ['$event'])
  handleKeyboardEvent(event: KeyboardEvent) {
    if (event.key === 'Escape') {
      this.closeUserMenu();
    }
  }

  constructor(private readonly elementRef: ElementRef) {}
}
