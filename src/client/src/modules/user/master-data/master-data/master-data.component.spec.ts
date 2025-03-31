import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MasterDataListComponent } from './master-data.component';


describe('MasterDataListComponent', () => {
  let component: MasterDataListComponent<any>;
  let fixture: ComponentFixture<MasterDataListComponent<any>>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MasterDataListComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MasterDataListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
