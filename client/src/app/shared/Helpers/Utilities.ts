import { HttpParams } from "@angular/common/http";
import { ShopParams } from "../models/shopParams";

export class Utilities {
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
