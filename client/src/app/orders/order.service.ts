import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { IOrder } from '../shared/models/order';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  baseUrl = environment.apiUrl;
  constructor(private httpClient: HttpClient) { }
  getOrdersForUser() {
    return this.httpClient.get<IOrder[]>(`${this.baseUrl}Orders`);
  }

  getOrderDetailed(id: number) {
    return this.httpClient.get<IOrder>(`${this.baseUrl}Orders/${id}`);
  }
}
