import { Component, OnInit } from '@angular/core';
import { AccountService } from './account/account.service';
import { BasketService } from './basket/basket.service';
import { IUser } from './shared/models/user';
import { IBasket } from './shared/models/basket';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {

  title = 'Skinet';
  constructor(private basketService: BasketService, private accountService: AccountService) {

  }
  ngOnInit(): void {
    this.loadCurrentUser();
    this.loadBasket();
  }

  loadCurrentUser() {
    const token = localStorage.getItem('token');
    this.accountService.loadCurrentUser(token).subscribe({
      next: (user: IUser) => console.log(`Load UserName:${user.displayName}`),
      error: (error: Error) => console.error(error)
    });
  }

  loadBasket() {
    const basketId = localStorage.getItem('basket_id');
    if (basketId) {
      this.basketService.getBasket(basketId).subscribe({
        next: (basket: IBasket) => console.log(`Initialize Basket With Items ${JSON.stringify(basket)}`),
        error: (error: Error) => console.error(error)
      })
    }
  }
}

