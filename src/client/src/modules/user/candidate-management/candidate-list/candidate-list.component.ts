import { CommonModule } from "@angular/common";
import { Component, OnInit, Inject } from "@angular/core";
import { ReactiveFormsModule, FormsModule, FormGroup, FormControl } from "@angular/forms";
import { FontAwesomeModule } from "@fortawesome/angular-fontawesome";
import { CANDIDATE_SERVICE } from "../../../../constants/injection.constant";
import { TableComponent } from "../../../../core/components/table/table.component";
import { Status } from "../../../../core/enums/candidate-status";
import { Gender } from "../../../../core/enums/gender";
import { TableColumn } from "../../../../core/models/table/table-column.model";
import { CandidateModel } from "../../../../models/candidate/candidate.model";
import { ICandidateService } from "../../../../services/candidate/candidate-service.interface";
import { ServicesModule } from "../../../../services/service.module";
import { MasterDataListComponent } from "../../master-data/master-data/master-data.component";
import { Router } from "@angular/router";

@Component({
  selector: 'app-candidate-list',
  imports: [CommonModule, ServicesModule, FontAwesomeModule, ReactiveFormsModule, FormsModule, TableComponent],
  templateUrl: './candidate-list.component.html',
  styleUrl: './candidate-list.component.css'
})
export class CandidateListComponent
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
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    private router: Router
  ) {
    super();
  }

  public statuses: { value: number; label: string }[] = [];

  override ngOnInit(): void {
    super.ngOnInit();
    this.statuses = Object.keys(Status)
    .filter(key => !isNaN(Number(Status[key as any]))) 
    .map(key => ({
      value: Status[key as keyof typeof Status],
      label: key.replace(/([A-Z])/g, ' $1').trim() 
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
          gender: Gender[item.gender as keyof typeof Gender],
          status: Status[item.status as keyof typeof Status],
        })),
      };
      console.log(this.data);
    });
  }

  public create(): void {
    this.router.navigate((['/candidate/create']));
  }
}
