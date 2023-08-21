import { Component, OnInit, Output, EventEmitter, Input } from '@angular/core';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketItem } from '../../models/basket';
import { Observable } from 'rxjs';
@Component({
  selector: 'app-basket-summery',
  templateUrl: './basket-summery.component.html',
  styleUrls: ['./basket-summery.component.scss']
})
export class BasketSummeryComponent implements OnInit {
  @Output() decrement: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() increment: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Output() remove: EventEmitter<IBasketItem> = new EventEmitter<IBasketItem>();
  @Input() isBasket = true;
  basket$: Observable<IBasket>;
  constructor(private basketService: BasketService) { }
  ngOnInit(): void {
    this.basket$ = this.basketService.basket$;
  }
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
