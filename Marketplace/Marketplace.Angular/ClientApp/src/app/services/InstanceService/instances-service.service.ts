import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {InstanceModel} from "../../models/InstanceModel";
import {ApiEndpointsConsts} from "../../helpers/ApiEndpoints";

@Injectable({
  providedIn: 'root'
})
export class InstancesServiceService {
  constructor(private httpClient: HttpClient) {

  }

  public getInstances = () : Observable<InstanceModel[]> => {
    const url = `${ApiEndpointsConsts.ServicesMethods.GetInstancesStatuses}`;
    return this.httpClient.get<InstanceModel[]>(url);
  }
}
