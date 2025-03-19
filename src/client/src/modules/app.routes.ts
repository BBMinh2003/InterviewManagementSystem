import { Routes } from '@angular/router';
import { UserLayoutComponent } from './shared/layouts/user-layout/user-layout.component';
import { LoginComponent } from './auth/login/login.component';
import { ForgotPasswordComponent } from './auth/forgot-password/forgot-password.component';

export const routes: Routes = [
  {
    path: 'login',
    component: LoginComponent // Route riêng cho login, không nằm trong layout
  },
  { path: 'forgot-password', 
    component: ForgotPasswordComponent 
  },
  {
    path: '',
    component: UserLayoutComponent,
    loadChildren: () => 
        import('./user/user.module').then((m) => m.UserModule),
  },
];
