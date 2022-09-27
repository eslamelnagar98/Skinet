import { Component, OnInit } from '@angular/core';
import { IBrand } from '../shared/models/brands';
import { IPagination } from '../shared/models/pagination';
import { IProduct } from '../shared/models/Product';
import { IType } from '../shared/models/ProductType';
import { ShopParams } from '../shared/models/shopParams';
import { ShopService } from './shop.service';

@Component({
  selector: 'app-shop',
  templateUrl: './shop.component.html',
  styleUrls: ['./shop.component.scss']
})
export class ShopComponent implements OnInit {

  products: Array<IProduct>;
  brands: Array<IBrand>;
  types: Array<IType>;
  shopParams = new ShopParams();
  totalCount:number;
  sortOptions = [
    { 'name': 'Alphabetical', value: 'name' },
    { 'name': 'Price: Low To High', value: 'priceAsc' },
    { 'name': 'Price: High To Low', value: 'priceDesc' },
  ];
  constructor(private shopService: ShopService) { }

  ngOnInit() {
    this.getProducts();
    this.getBrands();
    this.getTypes();
  }

  getProducts() {
    this.shopService.getProducts(this.shopParams).subscribe({
      next: (response: IPagination) => {
        this.products = response.data;
        this.shopParams.pageNumber = response.pageIndex;
        this.shopParams.pageSize = response.pageSize;
        this.totalCount=response.count;
      },
      error: (error) => console.error(error)
    }
    )
  }

  getBrands() {
    this.shopService.getBrands().subscribe({
      next: (response) => this.brands = [{ id: 0, name: 'All' }, ...response],
      error: (error) => console.error(error)
    })
  }

  getTypes() {
    this.shopService.getTypes().subscribe({
      next: (response) => this.types = [{ id: 0, name: 'All' }, ...response],
      error: (error) => console.error(error)
    })
  }

  onBrandSelected(brandId: number) {
    this.shopParams.brandId = brandId;
    this.getProducts();
  }
  onTypeSelected(typeId: number) {
    this.shopParams.typeId = typeId;
    this.getProducts();
  }

  onSortSelected(sort: string) {
    this.shopParams.sort = sort;
    this.getProducts();
  }

  onPageChanged(event:any){
    this.shopParams.pageNumber=event.page;
    this.getProducts();
  }

}
