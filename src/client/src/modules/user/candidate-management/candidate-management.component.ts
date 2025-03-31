import { CommonModule } from '@angular/common';
import { Component, Inject, OnInit } from '@angular/core';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TableComponent } from '../../../core/components/table/table.component';
import { MasterDataListComponent } from '../master-data/master-data/master-data.component';
import { CandidateModel } from '../../../models/candidate/candidate.model';
import { TableColumn } from '../../../core/models/table/table-column.model';
import { CANDIDATE_SERVICE } from '../../../constants/injection.constant';
import { ICandidateService } from '../../../services/candidate/candidate-service.interface';
import { ServicesModule } from '../../../services/service.module';
import { Gender } from '../../../core/enums/gender';
import { Status } from '../../../core/enums/candidate-status';

@Component({
  selector: 'app-candidate-management',
  imports: [CommonModule, ServicesModule, FontAwesomeModule, ReactiveFormsModule, FormsModule, TableComponent],
  templateUrl: './candidate-management.component.html',
  styleUrl: './candidate-management.component.css'
})
export class CandidateManagementComponent
  extends MasterDataListComponent<CandidateModel>
  implements OnInit
{
  public override columns: TableColumn[] = [
    { name: 'Name', value: 'name' },
    { name: 'Email', value: 'email' },
    { name: 'Phone No.', value: 'phone' },
    { name: 'Current Position', value: 'positionName' },
    { name: 'Owner HR', value: 'recruiterOwnerName' },
    { name: 'Status', value: 'status' },
  ];

  constructor(
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService
  ) {
    super();
  }

  public statuses: { value: number; label: string }[] = [];

  override ngOnInit(): void {
    super.ngOnInit();
    this.statuses = Object.keys(Status)
    .filter(key => !isNaN(Number(Status[key as any]))) // loại bỏ giá trị ngược
    .map(key => ({
      value: Status[key as keyof typeof Status],
      label: key.replace(/([A-Z])/g, ' $1').trim() // Optional: tách PascalCase -> Pascal Case
    }));
  }


  protected override createForm(): void {
    this.searchForm = new FormGroup({
      keyword: new FormControl(''),
      status: new FormControl<number | null> (null)
    });
  }

  protected override searchData(): void {
    this.candidateService.search(this.filter).subscribe((res) => {
      this.data = {
        ...res,
        items: res.items.map((item) => ({
          ...item,
          gender: Gender[item.gender as keyof typeof Gender], // Convert numeric gender to string using the Gender enum
          status: Status[item.status as keyof typeof Status], // Convert numeric status to string using the Status enum
        })),
      };
      console.log(this.data); // Log the mapped data for debugging
    });
  }

  // public delete(id: string): void {
  //   this.categoryService.delete(id).subscribe((data) => {
  //     // Neu xoa duoc thi goi lai ham getData de load lai du lieu
  //     if (data) {
  //       this.searchData();
  //     }
  //   });
  // }

  // public edit(id: string): void {
  //   this.isShowDetail = false;
  //   setTimeout(() => {
  //     this.selectedItem = this.data.items.find((x) => x.id === id);
  //     this.isShowDetail = true;

  //     // Scroll into view
  //   }, 150);
  // }

  public create(): void {
    // this.isShowDetail = false;
    // setTimeout(() => {
    //   this.selectedItem = null;
    //   this.isShowDetail = true;

    //   // Scroll into view
    // }, 150);
  }
}
