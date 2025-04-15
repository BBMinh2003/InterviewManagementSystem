import { Component, Input } from '@angular/core';
import { ItemDetails } from '../../models/item-detail/detail-row.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-item-details',
  imports: [CommonModule],
  templateUrl: './item-details.component.html',
  styleUrl: './item-details.component.css'
})
export class ItemDetailsComponent {
  @Input() items: ItemDetails[] = [];
}
