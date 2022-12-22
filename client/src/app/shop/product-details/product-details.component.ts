import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { IProduct } from 'src/app/shared/models/Product';
import { BreadcrumbService } from 'xng-breadcrumb';
import { ShopService } from '../shop.service';

@Component({
  selector: 'app-product-details',
  templateUrl: './product-details.component.html',
  styleUrls: ['./product-details.component.scss']
})
export class ProductDetailsComponent implements OnInit {

  product: IProduct;
  constructor(private shopService: ShopService, private activatedRoute: ActivatedRoute,
    private breadCrumbService: BreadcrumbService) {

    this.breadCrumbService.set('@ProductDetails', ' ');
  }

  ngOnInit() {
    this.loadProduct();
  }

  loadProduct() {
    const routeParam = +this.activatedRoute.snapshot.paramMap.get('id');
    this.shopService.getProduct(routeParam).subscribe({
      next: (product: IProduct) => {
        this.product = product
        this.breadCrumbService.set('@ProductDetails', product.name)
      },
      error: (error) => console.error(error)
    });
  }
}
