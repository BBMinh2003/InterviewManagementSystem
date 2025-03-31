import { Component } from '@angular/core';
import { ItemDetail } from '../../../core/models/item-detail/detail-row.model';
import { ItemDetailComponent } from '../../../core/components/item-detail/item-detail.component';

@Component({
  selector: 'app-offer-management',
  imports: [ItemDetailComponent],
  templateUrl: './offer-management.component.html',
  styleUrl: './offer-management.component.css',
})
export class OfferManagementComponent {
  details: ItemDetail[] = [
    // Row 1
    {
      label: 'Candidate',
      value: ['Nguyễn Quanh Anh']
    },
    {
      label: 'Contract Type',
      value: ['Full-time']
    },
  
    // Row 2
    {
      label: 'Position',
      value: ['Developer']
    },
    {
      label: 'Level',
      value: ['Senior']
    },
  
    // Row 3
    {
      label: 'Approver',
      value: ['Nguyễn Hòa']
    },
    {
      label: 'Department',
      value: ['IT']
    },
  
    // Row 4
    {
      label: 'Interview Info',
      value: [
        'Interview SBA',
        'Interviewer: ThuyNT, HANN2, AnhLM1'
      ]
    },
    {
      label: 'Recruiter Owner',
      value: ['Dainv']
    },
  
    // Row 5
    {
      label: 'Contract Period',
      value: [
        'From 12/02/2022',
        'To 12/02/2023'
      ]
    },
    {
      label: 'Due Date',
      value: ['25/12/2021']
    },
  
    // Row 6
    {
      label: 'Interview Notes',
      value: [
        'Candidate is fully-match with',
        '5 years experience in IT',
        'industry'
      ]
    },
    {
      label: 'Basic Salary',
      value: ['25.000.000 VND']
    },
  
    // Row 7
    {
      label: 'Status',
      value: ['Waiting for Approval'],
      customClass: 'px-3 py-1 bg-yellow-100 text-yellow-800 rounded-full text-sm'
    },
    {
      label: 'Note',
      value: ['N/A']
    }
  ];
}
