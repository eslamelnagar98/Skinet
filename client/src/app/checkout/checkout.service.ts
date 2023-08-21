import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;
  constructor(private httpClient: HttpClient) { }

  getDeliveryMethods() {
    return this.httpClient.get(`${this.baseUrl}Orders/deliveryMethods`).pipe(
      map((deliveryMethod: IDeliveryMethod[]) => {
        return deliveryMethod.sort((firstDeliveryMethod, secondDeliveryMethod) => {
          return secondDeliveryMethod.price - firstDeliveryMethod.price
        });
      })
    )
  }
}
