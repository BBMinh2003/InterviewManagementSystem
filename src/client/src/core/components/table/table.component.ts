import { Component, EventEmitter, Input, Output } from '@angular/core';

import {
  FontAwesomeModule,
  IconDefinition,
} from '@fortawesome/angular-fontawesome';
import {
  faTrash,
  faPlus,
  faSearch,
  faEye,
  faPenToSquare,
  faHome
} from '@fortawesome/free-solid-svg-icons';

import { TableColumn } from '../../models/table/table-column.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginatedResult } from '../../../models/paginated-result.model';

@Component({
  selector: 'app-table',
  imports: [CommonModule, FontAwesomeModule, FormsModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css'
})
export class TableComponent {

  //#region Font Awesome icons
  public faTrash: IconDefinition = faTrash;
  public faPlus: IconDefinition = faPlus;
  public faSearch: IconDefinition = faSearch;
  public faEye: IconDefinition = faEye;
  public faPenToSquare: IconDefinition = faPenToSquare;
  public faHome: IconDefinition = faHome;
  //#endregion

  @Input() columns: TableColumn[] = [];
  @Input() public isShowNumber?: boolean = true;
  @Input() public currentPage: number = 1;
  @Input() public currentPageSize: number = 2;

  @Input() public data!: PaginatedResult<any>;

  @Output() public onEdit: EventEmitter<string> = new EventEmitter<string>();

  @Output() public onDelete: EventEmitter<string> = new EventEmitter<string>();

  @Output() public onPageSizeChange: EventEmitter<any> =
    new EventEmitter<any>();

  @Output() public onPageChange: EventEmitter<number> =
    new EventEmitter<number>();

  public generatePageInfo(): string {
    if (this.data) {
      return `Page ${this.currentPage} -
      ${this.currentPageSize * this.data.pageNumber > this.data.totalCount
          ? this.data.totalCount
          : this.currentPageSize * this.data.pageNumber
        } of ${this.data.totalCount}`;
    }

    return '';
  }

public calculateColspan(): number {
    let colspan = this.columns.length + 1;
    if (this.isShowNumber) {
      colspan++;
    }
    return colspan;
  }
}
