import { Observable } from "rxjs";
import DashboardView from "../../models/common/dashboard.model";

export interface IDashboardService  {
  getData(): Observable<DashboardView>
}
