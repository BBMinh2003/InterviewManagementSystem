import { Component, Input } from '@angular/core';
import { ItemDetail } from '../../models/item-detail/detail-row.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-item-detail',
  imports: [CommonModule],
  templateUrl: './item-detail.component.html',
  styleUrl: './item-detail.component.css'
})
export class ItemDetailComponent {
  @Input() items: ItemDetail[] = [];
}
