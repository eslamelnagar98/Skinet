import { HttpClient, HttpParams, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Basket, IBasket, IBasketItem, IBasketTotals } from '../shared/models/basket';
import { IProduct } from '../shared/models/Product';

@Injectable({
  providedIn: 'root'
})
export class BasketService {

  baseUrl = environment.apiUrl;
  endPointName = 'Basket';
  private basketSource = new BehaviorSubject<IBasket>(null);
  private basketTotalSource = new BehaviorSubject<IBasketTotals>(null);
  basket$ = this.basketSource.asObservable();
  basketTotals$ = this.basketTotalSource.asObservable();
  localStorageKey: string = 'basket_id';
  constructor(private httpClient: HttpClient) { }

  incrementQuantity(item: IBasketItem) {

    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.basketItems.findIndex(basketItem => basketItem.id == item.id);
    basket.basketItems[foundItemIndex].quantity++;
    this.setBasket(basket);
  }

  decrementQuantity(item: IBasketItem) {

    const basket = this.getCurrentBasketValue();
    const foundItemIndex = basket.basketItems.findIndex(basketItem => basketItem.id == item.id);
    const basketItemHasMoreThanOneElement = basket.basketItems[foundItemIndex].quantity > 1;
    if (basketItemHasMoreThanOneElement) {
      basket.basketItems[foundItemIndex].quantity--;
      this.setBasket(basket);
      return;
    }

    this.removeItemFromBasket(item);
  }

  removeItemFromBasket(item: IBasketItem) {
    const basket = this.getCurrentBasketValue();
    basket.basketItems = basket.basketItems.filter(basketItem => basketItem.id !== item.id);
    if (basket.basketItems.length >= 1) {
      this.setBasket(basket);
      return;
    }
    this.deleteBasket(basket);
  }

  deleteBasket(basket: IBasket) {
    let params: HttpParams = new HttpParams().append("basketId", basket.id);
    return this.httpClient.delete(`${this.baseUrl}${this.endPointName}`, { observe: 'response', params }).subscribe({
      next: () => {
        this.basketSource.next(null);
        this.basketTotalSource.next(null);
        localStorage.removeItem(this.localStorageKey)
      },
      error: (error) => console.error(error)
    })
  }

  getBasket(id: string) {
    let params: HttpParams = new HttpParams().append("basketId", id);
    return this.httpClient.get(`${this.baseUrl}${this.endPointName}`, { observe: 'response', params })
      .pipe(
        map((response: HttpResponse<IBasket>) => {
          const basket = response.body;
          this.basketSource.next(basket);
          this.calculateTotals();
        })
      );
  }

  setBasket(basket: IBasket) {
    return this.httpClient.post(`${this.baseUrl}${this.endPointName}`, basket).subscribe({
      next: (response: IBasket) => {
        this.basketSource.next(response);
        this.calculateTotals();
      },
      error: (error) => console.error(error)
    });
  }

  getCurrentBasketValue() {
    return this.basketSource.value;
  }

  addItemToBasket(item: IProduct, quantity = 1) {
    const itemToAdd: IBasketItem = this.mapProductItemToBasketItem(item, quantity);
    const basket = this.getCurrentBasketValue() ?? this.createBasket();
    basket.basketItems = this.updateBasketItem(basket.basketItems, itemToAdd, quantity);
    this.setBasket(basket);
  }

  private mapProductItemToBasketItem(item: IProduct, quantity: number): IBasketItem {
    return {
      id: item.id,
      productName: item.name,
      price: item.price,
      pictureUrl: item.pictureUrl,
      quantity,
      brand: item.productBrand,
      type: item.productType
    };
  }

  private createBasket() {
    const basket = new Basket();
    localStorage.setItem(this.localStorageKey, basket.id);
    return basket;
  }

  private updateBasketItem(basketItems: IBasketItem[], itemToAdd: IBasketItem, quantity: number): IBasketItem[] {
    const index = basketItems.findIndex(index => index.id === itemToAdd.id);
    if (index === -1) {
      itemToAdd.quantity = quantity;
      basketItems.push(itemToAdd);
      return basketItems;
    }
    basketItems[index].quantity += quantity;
    return basketItems;
  }

  private calculateTotals() {
    const basket = this.getCurrentBasketValue();
    const shipping = 0;
    const subtotal = basket.basketItems.reduce((previous, current) => previous + (current.price * current.quantity), 0) ?? 0;
    const total = subtotal + shipping;
    this.basketTotalSource.next({ shipping, total, subtotal });
  }
}
