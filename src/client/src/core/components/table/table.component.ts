import {
  Component,
  EventEmitter,
  Input,
  Output,
  SimpleChanges,
} from '@angular/core';

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
  faHome,
  faSort,
} from '@fortawesome/free-solid-svg-icons';

import { TableColumn } from '../../models/table/table-column.model';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PaginatedResult } from '../../../models/paginated-result.model';
import { OrderDirection } from '../../../models/search.model';

@Component({
  selector: 'app-table',
  imports: [CommonModule, FontAwesomeModule, FormsModule],
  templateUrl: './table.component.html',
  styleUrl: './table.component.css',
})
export class TableComponent {
  //#region Font Awesome icons
  public faTrash: IconDefinition = faTrash;
  public faPlus: IconDefinition = faPlus;
  public faSearch: IconDefinition = faSearch;
  public faEye: IconDefinition = faEye;
  public faPenToSquare: IconDefinition = faPenToSquare;
  public faHome: IconDefinition = faHome;
  public faSort: IconDefinition = faSort;
  //#endregion

  @Input() columns: TableColumn[] = [];
  @Input() public isShowNumber?: boolean = true;
  @Input() public currentPage: number = 1;
  @Input() public currentPageSize: number = 2;
  @Input() public role: string = '';
  @Input() public isShowDeleteAction: boolean = true;
  @Input() public isShowEditAction: boolean = true;
  @Input() public data!: PaginatedResult<any>;

  public currentSortField: string | null = null;
  public currentSortDirection: OrderDirection | null = null;
  pageSizes = [2, 5, 10, 20, 50];
  inputPage: number | null = null;

  @Output() public onEdit: EventEmitter<string> = new EventEmitter<string>();

  @Output() public onDelete: EventEmitter<string> = new EventEmitter<string>();

  @Output() public onPageSizeChange: EventEmitter<number> =
    new EventEmitter<number>();
  @Output() public onViewDetails: EventEmitter<string> =
    new EventEmitter<string>();
  @Output() public onPageChange: EventEmitter<number> =
    new EventEmitter<number>();
  @Output() public onSort: EventEmitter<{
    field: string;
    direction: OrderDirection;
  }> = new EventEmitter();

  public isModalOpen: boolean = false;
  private deleteItemId: string | null = null;

  confirmDelete(itemId: string) {
    this.deleteItemId = itemId;
    this.isModalOpen = true;
  }

  closeModal() {
    this.isModalOpen = false;
    this.deleteItemId = null;
  }

  deleteRecord() {
    if (this.deleteItemId) {
      this.onDelete.emit(this.deleteItemId);
    }
    this.closeModal();
  }

  public generatePageInfo(): string {
    if (this.data) {
      const start = (this.currentPage - 1) * this.currentPageSize + 1;
      const end = Math.min(
        this.currentPage * this.currentPageSize,
        this.data.totalCount
      );
      return `${start}-${end} of ${this.data.totalCount} items`;
    }
    return '';
  }

  onPageSizeChangeHandler(newSize: number): void {
    this.onPageSizeChange.emit(newSize);
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (changes['currentPage']) {
      this.inputPage = this.currentPage;
    }
  }

  validateAndGoToPage(): void {
    if (!this.data) return;

    let page = Number(this.inputPage);

    if (isNaN(page) || page < 1) {
      page = 1;
    } else if (page > this.data.totalPages) {
      page = this.data.totalPages;
    }

    if (page !== this.currentPage) {
      this.currentPage = page;
      this.onPageChange.emit(page);
    }

    this.inputPage = page;
  }

  public getPages(): (number | string)[] {
    if (!this.data || this.data.totalPages <= 1) return [];

    const current = this.currentPage;
    const total = this.data.totalPages;
    const delta = 1;
    const range: number[] = [];
    const rangeWithDots: (number | string)[] = [];
    let prev = 0;

    for (let i = 1; i <= total; i++) {
      if (
        i === 1 ||
        i === total ||
        (i >= current - delta && i <= current + delta)
      ) {
        range.push(i);
      }
    }

    range.forEach((currentPage) => {
      if (prev && currentPage - prev !== 1) {
        rangeWithDots.push('...');
      }
      rangeWithDots.push(currentPage);
      prev = currentPage;
    });

    return rangeWithDots;
  }

  public calculateColspan(): number {
    let colspan = this.columns.length + 1;
    if (this.isShowNumber) {
      colspan++;
    }
    return colspan;
  }

  handleViewDetails(id: string): void {
    this.onViewDetails.emit(id);
  }

  handUpdate(id: string): void {
    this.onEdit.emit(id);
  }

  onSortColumn(field: string): void {
    const column = this.columns.find((col) => col.value === field);

    if (!column?.sortable) {
      return; 
    }

    if (this.currentSortField === field) {
      this.currentSortDirection =
        this.currentSortDirection === OrderDirection.ASC
          ? OrderDirection.DESC
          : OrderDirection.ASC;
    } else {
      this.currentSortField = field;
      this.currentSortDirection = OrderDirection.ASC;
    }

    this.onSort.emit({
      field: this.currentSortField,
      direction: this.currentSortDirection,
    });
  }
}
