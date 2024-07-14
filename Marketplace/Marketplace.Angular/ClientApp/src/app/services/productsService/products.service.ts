import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {ProductModel} from "../../models/products/productModel";
import {ApiEndpointsConsts} from "../../helpers/ApiEndpoints";

@Injectable({
  providedIn: 'root'
})
export class ProductsService {

  constructor(private httpClient: HttpClient) {

  }

  public getProductsOfTheDay = () : Observable<ProductModel[]> => {
    const url = `${ApiEndpointsConsts.ServicesMethods.GetProductsOfTheDay}`;
    return this.httpClient.get<ProductModel[]>(url);
  }
}
