import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { BasketService } from 'src/app/basket/basket.service';
import { IBasket, IBasketItem } from 'src/app/shared/models/basket';
import { IProduct } from 'src/app/shared/models/Product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';
import { lastValueFrom } from 'rxjs';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  quantity: number = 1
  product: IProduct;
  constructor(private shopService: ShopService,
    private activatedRoute: ActivatedRoute,
    private breadCrumbService: BreadcrumbService,
    private basketService: BasketService) {
    this.breadCrumbService.set('@ProductDetails', ' ');
  }

  async ngOnInit() {
    await this.loadProduct();
    this.updateBasketItemQuantity();
  }

  async loadProduct() {
    const routeParam = +this.activatedRoute.snapshot.paramMap.get('id');
    this.product = await lastValueFrom(this.shopService.getProduct(routeParam));
    this.breadCrumbService.set('@ProductDetails', this.product.name)
  }

  addItemToBasket() {
    console.log(`Product price ${this.product.price} :: Quantity ${this.quantity}`);
    this.basketService.addItemToBasket(this.product, this.quantity);
  }
  incrementItemQuantity() {
    this.quantity++;
  }

  decrementItemQuantity() {
    if (this.quantity === 1) return;
    this.quantity--;

  }
  updateBasketItemQuantity() {
    this.basketService.basket$.subscribe({
      next: (basket: IBasket) => {
        if (basket === null) return;
        const foundItemIndex = basket.basketItems.findIndex(item => item.id == this.product.id)
        if (foundItemIndex === -1) return;
        this.quantity = basket.basketItems[foundItemIndex]?.quantity ?? 1;
      }
    })
  }
}
