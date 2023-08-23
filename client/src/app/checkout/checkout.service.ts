import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { IDeliveryMethod } from '../shared/models/deliveryMethod';
import { IOrder, IOrderToCreate } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class CheckoutService {
  baseUrl = environment.apiUrl;
  constructor(private httpClient: HttpClient) { }

  createOrder(order: IOrderToCreate) {
    return this.httpClient.post<IOrderToCreate>(`${this.baseUrl}Orders`, order);
  }
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
