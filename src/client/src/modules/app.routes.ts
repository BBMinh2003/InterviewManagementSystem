import { Routes } from '@angular/router';
import { UserLayoutComponent } from './shared/layouts/user-layout/user-layout.component';

export const routes: Routes = [
  {
    path: '',
    component: UserLayoutComponent,
    loadChildren: () => 
        import('./user/user.module').then((m) => m.UserModule),
  },
];
