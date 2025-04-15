import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { InterviewDetailsComponent } from './interview-details/interview-details.component';
import { InterviewManagementComponent } from './interview-list/interview-management.component';
import { InterviewCreateComponent } from './interview-create/interview-create.component';
import { InterviewEditComponent } from './interview-edit/interview-edit.component';
import { canEditInterview } from '../../../guards/edit-interview.guard';
import { canActivateInterview } from '../../../guards/interview.guard';
import { Title } from 'chart.js';

const routes: Routes = [
  { path: 'create', component: InterviewCreateComponent,canActivate: [canEditInterview],data: {title: 'Create interview' } },
  { path: 'interview-edit/:id', component: InterviewEditComponent,canActivate: [canEditInterview] , data: {title: 'Update interview ' }  }, 
  { path: 'interview-detail/:id', component: InterviewDetailsComponent, canActivate: [canActivateInterview],data: {title: 'Interview details' } },
  { path: '', component: InterviewManagementComponent,data: {title: 'Interview' } },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class InterviewModule {}
