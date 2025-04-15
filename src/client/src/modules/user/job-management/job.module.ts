import { RouterModule, Routes } from "@angular/router";
import { JobDetailsComponent } from "./job-details/job-details.component";
import { JobManagementComponent } from "./job-list/job-management.component";
import { NgModule } from "@angular/core";
import { JobCreateComponent } from "./job-create/job-create.component";
import { JobUpdateComponent } from "./job-update/job-update.component";
import { canActivateTeamAdmin } from "../../../guards/admin.guard";


const routes: Routes = [
  {
    path: 'create',
    canActivate: [canActivateTeamAdmin],
    component: JobCreateComponent },
  {
    path: 'update/:id',
    canActivate: [canActivateTeamAdmin],
    component: JobUpdateComponent },
  { path: 'job/:id', component: JobDetailsComponent },
  { path: '', component: JobManagementComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class JobModule {}
