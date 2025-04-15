import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home/home.component';
import { RouterModule, Routes } from '@angular/router';
import { JobManagementComponent } from './job-management/job-list/job-management.component';
import { InterviewManagementComponent } from './interview-management/interview-list/interview-management.component';
import { OfferManagementComponent } from './offer-management/offer-list/offer-management.component';
import { UserManagementComponent } from '../admin/user-management/user-management.component';
import { canActivateTeamAdmin } from '../../guards/admin.guard';
import { CandidateListComponent } from './candidate-management/candidate-list/candidate-list.component';
import { CandidateCreateComponent } from './candidate-management/candidate-create/candidate-create.component';
import { OfferDetailsComponent } from './offer-management/offer-details/offer-details.component';
import { JobDetailsComponent } from './job-management/job-details/job-details.component';
import { CandidateDetailsComponent } from './candidate-management/candidate-details/candidate-details.component';
import { CandidateUpdateComponent } from './candidate-management/candidate-update/candidate-update.component';
import { canActivateTeamOffer } from '../../guards/offer.guard';
import { OfferCreateComponent } from './offer-management/offer-create/offer-create.component';
import { OfferEditComponent } from './offer-management/offer-edit/offer-edit.component';
import { canActivateInterview } from '../../guards/interview.guard';
import { canActivateTeam } from '../../guards/authenticated.guard';
import { ProfileComponent } from './profile/profile.component';
import { ProfileEditComponent } from './profile-edit/profile-edit.component';

const routes: Routes = [
  { path: 'home', component: HomeComponent, data: { title: 'Home' } },
  {
    path: 'candidate',
    children: [
      {
        path: '', component: CandidateListComponent, data: { title: 'Candidate' } },
      {
        path: 'create',
        component: CandidateCreateComponent,
        canActivate: [canActivateTeamAdmin], 
        data: { title: 'Create Candidate' } }, 
      {
        path: ':id',
        component: CandidateDetailsComponent,
        data: { title: 'Candidate Details' } }, 
      {
        path: 'update/:id',
        component: CandidateUpdateComponent,
        canActivate: [canActivateTeamAdmin], 
        data: { title: 'Candidate Details' } }, 
    ],
  },
  { path: 'job', component: JobManagementComponent, data: { title: 'Job' } },
  {
    path: 'interview',
    canActivate: [canActivateInterview],
    component: InterviewManagementComponent,
    data: { title: 'Interview' },
  },
  {
    path: 'offer',
    component: OfferManagementComponent,
    canActivate: [canActivateTeamOffer],
    loadChildren: () => import('./offer-management/offer.module').then(m => m.OfferModule),
    data: { title: 'Offer' },
  },
  {
    path: 'job/:id',
    component: JobDetailsComponent,
    data: { title: 'Job Details' },
  },
  {
    path: 'offer/create',
    component: OfferCreateComponent,
    data: { title: 'Offer Create' },
  },
  {
    path: 'offer/:id',
    component: OfferDetailsComponent,
    canActivate: [canActivateTeamOffer],
    data: { title: 'Offer Details' },
  },
  {
    path: 'offer/edit/:id',
    component: OfferEditComponent,
    canActivate: [canActivateTeamOffer],
    data: { title: 'Offer Edit' },
  },
  {
    path: 'profile',
    component: ProfileComponent,
    canActivate: [canActivateTeam],
    data: { title: 'Profile' },
  },
  {
    path: 'profile/edit',
    component: ProfileEditComponent,
    canActivate: [canActivateTeam],
    data: { title: 'Profile Edit' },
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
