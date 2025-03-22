import { Routes } from '@angular/router';
import { UserLayoutComponent } from './shared/layouts/user-layout/user-layout.component';
import { LoginComponent } from './auth/login/login.component';
import { ResetPasswordComponent } from './auth/reset-password/reset-password.component';
import { canActivateTeam } from '../guards/authenticated.guard';
import { anonymousGuard } from '../guards/anonymous.guard';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [anonymousGuard]
  },
  {
    path: 'reset-password',
    component: ResetPasswordComponent,
    canActivate: [anonymousGuard]
  },
  {
    path: '',
    component: UserLayoutComponent,
    canActivate: [canActivateTeam],
    loadChildren: () =>
      import('./user/user.module').then((m) => m.UserModule),
  },
];
