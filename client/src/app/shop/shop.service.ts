import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { IBrand } from '../shared/models/brands';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/Product';
import { IType } from '../shared/models/ProductType';
import { ShopParams } from '../shared/models/shopParams';
@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = 'https://localhost:5001/api/';
  pageSize = 6;
  constructor(private http: HttpClient) { }
  getProducts(shopParams: ShopParams) {
    shopParams.pageSize = this.pageSize;
    let params = this.concatQueryParams(shopParams);
    return this.http.get<IPagination>(`${this.baseUrl}products`, { observe: 'response', params })
      .pipe(
        map(response => {
          return response.body;
        }))
  }

  getProduct(productId: number) {
    return this.http.get<IProduct>(`${this.baseUrl}products/${productId}`);
  }

  getBrands() {
    return this.http.get<IBrand[]>(`${this.baseUrl}products/brands`);
  }

  getTypes() {
    return this.http.get<IType[]>(`${this.baseUrl}products/types`);
  }





  concatQueryParams(queryParams: ShopParams): HttpParams {
    let params: HttpParams = new HttpParams();
    Object.keys(queryParams).forEach(key => {
      if (queryParams.hasOwnProperty(key) && queryParams[key]) {
        const value = queryParams[key];
        if (value !== 0) {
          if (key === 'pageNumber') {
            params = params.append('pageIndex', value.toString());
          }
          else {
            params = params.append(key, value.toString());
          }
        }
      }
    });
    return params
  }


}
