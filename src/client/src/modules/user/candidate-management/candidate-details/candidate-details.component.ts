import { CommonModule } from '@angular/common';
import { faHome, IconDefinition } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { Component, Inject, OnInit } from '@angular/core';
import { AUTH_SERVICE, CANDIDATE_SERVICE, NOTIFICATION_SERVICE } from '../../../../constants/injection.constant';
import { ICandidateService } from '../../../../services/candidate/candidate-service.interface';
import { ActivatedRoute, Router } from '@angular/router';
import { CandidateModel } from '../../../../models/candidate/candidate.model';
import { CandidateService } from '../../../../services/candidate/candidate.service';
import { Status } from '../../../../core/enums/candidate-status';
import { HighestLevel } from '../../../../core/enums/highestLevel';
import { Gender } from '../../../../core/enums/gender';
import { IAuthService } from '../../../../services/auth/auth-service.interface';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationDialogComponent } from '../../../../core/components/confirmation-dialog/confirmation-dialog.component';
import { INotificationService } from '../../../../services/notification/notification-service.interface';

@Component({
  selector: 'app-candidate-details',
  imports: [CommonModule, FontAwesomeModule],
  templateUrl: './candidate-details.component.html',
  styleUrl: './candidate-details.component.css',
  providers: [
    { provide: CANDIDATE_SERVICE, useClass: CandidateService },
  ]
})
export class CandidateDetailsComponent implements OnInit {
  public faHome: IconDefinition = faHome;

  role: any;
  public candidate?: CandidateModel | any;
  public cvFileUrl!: string;

  constructor(
    @Inject(CANDIDATE_SERVICE) private candidateService: ICandidateService,
    @Inject(AUTH_SERVICE) private authService: IAuthService,
    @Inject(NOTIFICATION_SERVICE) private notificationService: INotificationService,
    private router: Router,
    private route: ActivatedRoute,
    public dialog: MatDialog,
  ) { }

  ngOnInit(): void {
    this.authService.getUserInformation().subscribe((res) => {
      this.role = res?.role;
    });

    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadCandidate(id);
    }
  }

  private loadCandidate(id: string): void {
    this.candidateService.getById(id).subscribe({
      next: (data) => {
        this.candidate = {
          ...data,
          status: Status[data.status as keyof typeof Status].toString().replace(/([A-Z])/g, ' $1').trim(),
          gender: Gender[data.gender as keyof typeof Gender],
          highestLevel: HighestLevel[data.highestLevel as keyof typeof HighestLevel].toString().replace(/([A-Z])/g, ' $1').trim(),
        };

        console.log(typeof this.candidate.cvFile);

        if (this.candidate) {
          this.cvFileUrl = this.candidate.cvFile!;
        }
        else {
          console.log('Error');

        }

      },
      error: (err) => {
        console.error('Error fetching interview details', err);
        this.router.navigate(['/candidate']);
      },
    });
  }

  public openCv(cvFile: string): void {
    const link = document.createElement('a');
    link.href = cvFile;
    link.download = this.candidate.cV_Attachment;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);

  }

  banCandidate() {
    const dialogRef = this.dialog.open(ConfirmationDialogComponent);
    console.log(this.candidate.id);

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.candidateService.banCandidate(this.candidate.id!).subscribe({
          next: (response) => {
            if (response === true) {
              console.log('Success');
              
              this.notificationService.showMessage(
                'Ban candidate successfully!',
                'success'
              );
              this.loadCandidate(this.candidate.id!);
            }
            else {
              console.log('Erroer');
              this.notificationService.showMessage(
                'Candidate already banned!',
                'warning',
              );
            }
          },
          error: () => {
            this.notificationService.showMessage('Error banning candidate', 'error');
          },
        });
      }
    }
    );
  }

  editCandidate() {
    this.router.navigate(['candidate/update', this.candidate.id!]);
  }

  cancel() {
    this.router.navigate(['candidate']);
  }
}
