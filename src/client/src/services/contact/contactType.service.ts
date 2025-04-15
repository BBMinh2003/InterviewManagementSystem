import { Injectable } from '@angular/core';
import { MasterDataService } from '../master-data/master-data.service';
import { ContactTypeModel } from '../../models/contactType/contactType.model';
import { HttpClient } from '@angular/common/http';
import { IContactTypeService } from './contact-service.interface';

@Injectable({
  providedIn: 'root',
})
export class ContactTypeService
  extends MasterDataService<ContactTypeModel>
  implements IContactTypeService
{
  constructor(protected override httpClient: HttpClient) {
    super(httpClient, 'ContactType');
  }
  getAllContactTypes() {
    return this.httpClient.get<{ id: string; name: string }[]>(this.baseUrl);
  }
}
