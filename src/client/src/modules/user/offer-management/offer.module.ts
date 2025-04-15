import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { OfferDetailsComponent } from './offer-details/offer-details.component';
import { OfferManagementComponent } from './offer-list/offer-management.component';
import { OfferCreateComponent } from './offer-create/offer-create.component';

const routes: Routes = [
  // { path: 'offer-detail/:id', component: OfferDetailComponent },
  // { path: 'create', component: OfferCreateComponent },
  // { path: '', component: OfferManagementComponent },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class OfferModule {}
