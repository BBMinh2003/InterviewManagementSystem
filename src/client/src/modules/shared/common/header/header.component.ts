import { CommonModule } from '@angular/common';
import { Component, OnDestroy } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router } from '@angular/router';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome'
import { filter, map, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-header',
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent  implements OnDestroy {
  pageTitle = '';
  private readonly destroy$ = new Subject<void>();

  constructor(private readonly router: Router, private readonly activatedRoute: ActivatedRoute) {
    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd),
      map(() => this.getDeepestChild(this.activatedRoute)),
      takeUntil(this.destroy$)
    ).subscribe((route: ActivatedRoute) => {
      this.pageTitle = route.snapshot.data['title'] || 'Dashboard';
    });
  }

  private getDeepestChild(route: ActivatedRoute): ActivatedRoute {
    while (route.firstChild) {
      route = route.firstChild;
    }
    return route;
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }
}
