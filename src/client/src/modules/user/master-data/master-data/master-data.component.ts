import { IconDefinition } from '@fortawesome/angular-fontawesome';
import { faPlus, faRotate, faSearch, faHome } from '@fortawesome/free-solid-svg-icons';
import { FormGroup } from '@angular/forms';

import { Component } from '@angular/core';
import { PaginatedResult } from '../../../../models/paginated-result.model';
import { TableColumn } from '../../../../core/models/table/table-column.model';
import { OrderDirection, SearchModel } from '../../../../models/search.model';

@Component({
  selector: 'app-master-data-list',
  templateUrl: './master-data.component.html',
  styleUrls: ['./master-data.component.css'],
})
export class MasterDataListComponent<T> {
  //#region Font Awesome icons
  public faPlus: IconDefinition = faPlus;
  public faSearch: IconDefinition = faSearch;
  public faRotate: IconDefinition = faRotate;
  public faHome: IconDefinition = faHome;

  //#endregion

  public isShowDetail: boolean = false;
  public selectedItem!: T | undefined | null;
  
  public filter: SearchModel = {
    keyword: '',
    pageNumber: 1,
    pageSize: 5,
    orderBy: 'status',
    orderDirection: OrderDirection.ASC,
  };


  public currentPage: number = 1;
  public currentPageSize: number = 5;

  public searchForm!: FormGroup;

  public data!: PaginatedResult<T>;

  public columns: TableColumn[] = [
    { name: 'Name', value: 'name' },
    { name: 'Description', value: 'description' },
  ];

  constructor() {}

  ngOnInit(): void {
    this.createForm();
    this.searchData();
  }

  protected createForm(): void {}

  protected searchData(): void {}

  public onSubmit(): void {
    // Gan gia tri tu form vao filter => Keyword
    Object.assign(this.filter, this.searchForm.value);
    this.filter.pageNumber = 1;
    this.currentPage = 1;
    this.searchData();
  }

  public onCloseDetail(): void {
    console.log('Event send from detail');
    this.isShowDetail = false;
    this.searchData();
  }

  public onPageChange(page: number): void {
    this.filter.pageNumber = page;
    if (page < 0 || page > this.data.totalPages || page === this.currentPage) {
      return;
    }

    this.currentPage = page;
    this.searchData();
  }

  public onPageSizeChange(newSize: number): void {
    this.currentPageSize = newSize;
    this.filter.pageSize = newSize;
    this.filter.pageNumber = 1;
    this.currentPage = 1;
    this.searchData();
  }
}
