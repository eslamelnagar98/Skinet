import { Component, Output, EventEmitter, Input } from '@angular/core';
import { IBasketItem } from '../../models/basket';
import { IOrderItem } from '../../models/order';
@Component({
  selector: 'app-basket-summery',
  templateUrl: './basket-summery.component.html',
  styleUrls: ['./basket-summery.component.scss']
})
export class BasketSummeryComponent {
  @Input() items: IBasketItem[] | IOrderItem[] = [];
  @Input() isBasket = true;
  @Input() isOrder = false;
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  constructor() { }

  onBasketItemsDecrement(item: IBasketItem) {
    this.decrement.emit(item);
  }
  onBasketItemsIncrement(item: IBasketItem) {
    this.increment.emit(item);
  }
  onRemovedItemFromBasket(item: IBasketItem) {
    this.remove.emit(item);
  }
}
