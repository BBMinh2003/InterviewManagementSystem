import { Component, ElementRef, HostListener, Inject, Input } from '@angular/core';
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
import { Router, RouterModule } from '@angular/router';
import { AUTH_SERVICE, NOTIFICATION_SERVICE } from '../../../../constants/injection.constant';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { UserInformation } from '../../../../models/auth/user-information.model';
import { Subscription } from 'rxjs';
import { INotificationService } from '../../../../services/notification/notification-service.interface';
import { NotificationComponent } from "../../../../core/components/notification/notification.component";

@Component({
  selector: 'app-sidebar',
  imports: [CommonModule, FaIconComponent, MatIconModule, RouterModule, NotificationComponent],
  templateUrl: './sidebar.component.html',
  styleUrl: './sidebar.component.css',
})
export class SidebarComponent {
  public userInfo: UserInformation | null = null;
  private userSubscription!: Subscription;
  open = true;
  isMobile: boolean = window.innerWidth < 768;
  @Input() isMobileMenuOpen: boolean = false;

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


  toggleMobileMenu() {
    this.isMobileMenuOpen = !this.isMobileMenuOpen;
  }

  closeMobileMenu() {
    this.isMobileMenuOpen = false;
  }

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

    this.notificationService.showMessage('Logout successfully');
    this.authService.logout();
    this.router.navigate(['/login']);
  }

  closeUserMenu() {
    this.isUserMenuOpen = false;
  }

  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.isMobile = window.innerWidth < 768;
    if (this.isMobile) {
      this.open = true;
    }
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

  constructor(
    private readonly elementRef: ElementRef,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(NOTIFICATION_SERVICE) private notificationService: INotificationService,
    private router: Router
  ) {}

  ngOnInit() {
    this.userSubscription = this.authService
      .getUserInformation()
      .subscribe((user) => {
        this.userInfo = user;
      });
  }

  ngOnDestroy() {
    if (this.userSubscription) {
      this.userSubscription.unsubscribe();
    }
  }
}
