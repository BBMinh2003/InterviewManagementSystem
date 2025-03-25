import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { RouterModule, Routes } from '@angular/router';
import { CandidateManagementComponent } from './candidate-management/candidate-management.component';
import { JobManagementComponent } from './job-management/job-management.component';
import { InterviewManagementComponent } from './interview-management/interview-management.component';
import { OfferManagementComponent } from './offer-management/offer-management.component';
import { UserManagementComponent } from '../admin/user-management/user-management.component';
import { canActivateTeamAdmin } from '../../guards/admin.guard';

const routes: Routes = [
  { path: 'home', component: HomeComponent, data: { title: 'Home' } },
  {
    path: 'candidate',
    component: CandidateManagementComponent,
    data: { title: 'Candidate' },
  },
  { path: 'job', component: JobManagementComponent, data: { title: 'Job' } },
  {
    path: 'interview',
    component: InterviewManagementComponent,
    data: { title: 'Interview' },
  },
  {
    path: 'offer',
    component: OfferManagementComponent,
    data: { title: 'Offer' },
  },
  {
    path: 'user',
    component: UserManagementComponent,
    canActivate: [canActivateTeamAdmin],
    data: { title: 'User' },
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
})
export class UserModule {}
