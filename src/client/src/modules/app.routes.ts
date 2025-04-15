import { Routes } from '@angular/router';
import { UserLayoutComponent } from './shared/layouts/user-layout/user-layout.component';
import { LoginComponent } from './auth/login/login.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { canActivateTeam } from '../guards/authenticated.guard';
import { anonymousGuard } from '../guards/anonymous.guard';
import { ErrorPageComponent } from '../core/components/error-page/error-page.component';
import { canActivateTeamAdmin } from '../guards/admin.guard';
import { AdminLayoutComponent } from './shared/layouts/admin-layout/admin-layout.component';
import { canActivateInterview } from '../guards/interview.guard';

export const routes: Routes = [
  { path: 'login', component: LoginComponent },
  { path: 'reset-password', component: ResetPasswordComponent },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [anonymousGuard],
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent,
    canActivate: [anonymousGuard],
  },
  {
    path: 'admin',
    component: AdminLayoutComponent,
    canActivate: [canActivateTeamAdmin],
    loadChildren: () =>
      import('./admin/admin.module').then((m) => m.AdminModule),
  },
  {
    path: 'interview',
    component: UserLayoutComponent,
    canActivate: [canActivateInterview],
    loadChildren: () =>
      import('./user/interview-management/interview.module').then(
        (m) => m.InterviewModule
      ),
  },
  {
    path: 'job',
    component: UserLayoutComponent,
    canActivate: [canActivateTeam],
    loadChildren: () =>
      import('./user/job-management/job.module').then(
        (m) => m.JobModule
      ),
  },
  { path: 'error', component: ErrorPageComponent },
  {
    path: '',
    component: UserLayoutComponent,
    canActivate: [canActivateTeam],
    loadChildren: () => import('./user/user.module').then((m) => m.UserModule),
  },

  { path: '**', redirectTo: '/error?code=404' },
];
