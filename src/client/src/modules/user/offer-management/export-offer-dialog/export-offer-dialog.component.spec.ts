import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ExportOfferDialogComponent } from './export-offer-dialog.component';

describe('ExportOfferDialogComponent', () => {
  let component: ExportOfferDialogComponent;
  let fixture: ComponentFixture<ExportOfferDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ExportOfferDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ExportOfferDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
