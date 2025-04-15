import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule, Routes } from '@angular/router';
import { UserManagementComponent } from './user-management/user-management.component';
import { UserDetailsComponent } from './user-management/user-details/user-details.component';
import { UserCreateComponent } from './user-management/user-create/user-create.component';
import { UserEditComponent } from './user-management/user-edit/user-edit.component';

const routes: Routes = [
  {
    path: 'user',
    component: UserManagementComponent,
    data: { title: 'User' },
  },
  {
    path: 'user/create',
    component: UserCreateComponent,
    data: { title: 'Create User' },
  },
  {
    path: 'user/edit/:id',
    component: UserEditComponent,
    data: { title: 'User Details' },
  },
  {
    path: 'user/:id',
    component: UserDetailsComponent,
    data: { title: 'User Details' },
  },
  {
    path: '**',
    redirectTo: 'home',
    pathMatch: 'full',
    data: { title: 'Home' },
  },
];

@NgModule({
  declarations: [],
  imports: [CommonModule, RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminModule {

}
