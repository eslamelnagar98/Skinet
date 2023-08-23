import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable, map } from 'rxjs';
import { BasketService } from 'src/app/basket/basket.service';

@Injectable({
  providedIn: 'root'
})
export class BasketGuard implements CanActivate {
  constructor(private basketService: BasketService, private router: Router) { }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> {
    return this.basketService.basket$.pipe(
      map(basket => {
        if (basket) {
          return true;
        }
        this.router.navigate(['/shop'], { queryParams: { returnUrl: state.url } });
      })
    );
  }

}
