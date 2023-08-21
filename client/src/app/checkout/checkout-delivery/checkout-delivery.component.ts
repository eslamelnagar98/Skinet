import { Component, OnInit, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { CheckoutService } from '../checkout.service';
import { IDeliveryMethod } from 'src/app/shared/models/deliveryMethod';
@Component({
  selector: 'app-checkout-delivery',
  templateUrl: './checkout-delivery.component.html',
  styleUrls: ['./checkout-delivery.component.scss']
})
export class CheckoutDeliveryComponent implements OnInit {
  @Input() checkoutForm: FormGroup;
  deliveryMethods: IDeliveryMethod[];
  constructor(private checkoutService: CheckoutService) { }

  ngOnInit(): void {
    this.checkoutService
      .getDeliveryMethods()
      .subscribe({
        next: (deliveryMethodsInput: IDeliveryMethod[]) => this.deliveryMethods = deliveryMethodsInput,
        error: (error) => console.log(error)
      });
  }
}
