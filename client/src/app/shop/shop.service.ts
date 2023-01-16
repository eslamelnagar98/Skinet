import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Utilities } from '../shared/Helpers/Utilities';
import { IBrand } from '../shared/models/brands';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/Product';
import { IType } from '../shared/models/ProductType';
import { ShopParams } from '../shared/models/shopParams';
@Injectable({
  providedIn: 'root'
})
export class ShopService {

  baseUrl = environment.apiUrl;
  pageSize = 6;
  constructor(private http: HttpClient) { }
  getProducts(shopParams: ShopParams) {
    shopParams.pageSize = this.pageSize;
    let utilities = new Utilities();
    let params = utilities.concatQueryParams(shopParams);
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

}
