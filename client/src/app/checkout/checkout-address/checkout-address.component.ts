import { Component, Input } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from 'src/app/account/account.service';
import { IAddress } from 'src/app/shared/models/address';

@Component({
  selector: 'app-checkout-address',
  templateUrl: './checkout-address.component.html',
  styleUrls: ['./checkout-address.component.scss']
})
export class CheckoutAddressComponent {
  @Input() checkoutForm: FormGroup;
  constructor(private accountService: AccountService, private toastr: ToastrService) { }
  setAddressAsDefaultAddress() {
    const address: IAddress = this.checkoutForm.get('addressForm')?.value;
    if (address === null) {
      this.toastr.error('Invalid Address Form Data')
    }
    this.updateAddress(address);
  }

  private updateAddress(address: IAddress) {
    this.accountService.updateUserAddress(address).subscribe({
      next: _ => {
        this.toastr.success('Address saved');
      },
      error: (error: Error) => {
        this.toastr.error(error.message);
        console.log(error);
      }
    })
  }
}
